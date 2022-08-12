using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RemoteConfig;
using Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundle
{
    public class AssetBundleProvider : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Network network;
#pragma warning restore 649

        #region Constants
        private const string ContentLengthHeader = "Content-Length";
        private const float CalculatingAverageSpeedDelay = 1f;
        #endregion
        
        private Coroutine _loadBundlesCoroutine;
        private Coroutine _loadBundlesFromCacheCoroutine;
        private UnityWebRequest _unityWebRequest;
        private (float previos, float current) _downloadedBytesPerSecond;
        private readonly List<float> _downloadedBytesPerSecondList = new List<float>();
        private List<AssetBundle> _assetBundlesList = new List<AssetBundle>();
        private static int _bundlesSizeInBytes;
        public static int DownloadedBundleBytes { get; private set; }
        public static TimeSpan RemainDownloadingTime { get; private set; } = TimeSpan.Zero;
        public static float AverageSpeed { get; private set; }

        public static int BundlesSizeInBytes
        {
            get => _bundlesSizeInBytes;
            private set
            {
                _bundlesSizeInBytes = value;
                BundlesSizeInMegabytes = InfoAbbreviationUtility
                    .ConvertByteTo(_bundlesSizeInBytes, InfoAbbreviationUtility.InfoAbbreviationEnum.Mb).value;
            }
        }
        public static float BundlesSizeInMegabytes { get; private set; }

#if UNITY_EDITOR
        [MenuItem("Custom/Clear Bundle Cache", priority = 1)]
        private static void ClearBundleCache()
        {
            Caching.ClearCache();
            Debug.Log("Bundle Cache is empty");
        }
#endif

        private void TryLoadBundlesFromCache(Action onBundlesLoaded, Action onBundlesStartLoading)
        {
            _loadBundlesFromCacheCoroutine ??= StartCoroutine(LoadFromCache(onBundlesLoaded, onBundlesStartLoading));
        }

        private IEnumerator LoadFromCache(Action onBundlesLoaded, Action onBundlesStartLoading)
        {
            while (!Caching.ready)
            {
                yield return null;
            }
            InitAvailableAssetBundles();
            foreach (var bundle in _assetBundlesList)
            {
                _unityWebRequest = UnityWebRequest.Get(bundle.BundleManifestUrl);
                yield return _unityWebRequest.SendWebRequest();
                if(_unityWebRequest.result != UnityWebRequest.Result.Success)
                    yield break;
                string hash = _unityWebRequest.downloadHandler.text.Split("\n".ToCharArray())[5];
                Hash128 bundleHash = Hash128.Parse(hash.Split(':')[1].Trim());
                if(!bundleHash.isValid)
                    yield break;
                bundle.Hash128 = bundleHash;
                bundle.IsCached = Caching.IsVersionCached(bundle.BundleUrl, bundle.Hash128);
            }
            _loadBundlesCoroutine ??= StartCoroutine(DownloadRequest(onBundlesLoaded, onBundlesStartLoading));
        }

        private void InitAvailableAssetBundles()
        {
            foreach (var bundleName in RemoteConfigController.AvailableAssetBundles.assetBundlesNames)
            {
                _assetBundlesList.Add(new AssetBundle(bundleName));
            }
        }

        public void LoadBundles(Action onBundlesLoaded, Action onBundlesStartLoading)
        {
            TryLoadBundlesFromCache(onBundlesLoaded, onBundlesStartLoading);
        }
        
        private IEnumerator RequestBundlesSize(Action onBundlesStartLoading)
        {
            int amountOfNonCached = 0;
            foreach (var assetBundle in _assetBundlesList)
            {
                if (!assetBundle.IsCached)
                {
                    amountOfNonCached++;
                    _unityWebRequest = UnityWebRequest.Head(assetBundle.BundleUrl);
                    yield return _unityWebRequest.SendWebRequest();
                    assetBundle.SizeInBytes = int.Parse(_unityWebRequest.GetResponseHeader(ContentLengthHeader));
                    BundlesSizeInBytes += assetBundle.SizeInBytes;
                    Debug.Log(assetBundle.BundleName + " Size=" + assetBundle.SizeInMegabytes);
                }
            }
            if (amountOfNonCached == 0)
                yield break;
            network.RequestDownloadData();
            while (!Network.IsDownloadingApproved)
            {
                yield return null;
            }
            onBundlesStartLoading.Invoke();
        }

        private IEnumerator DownloadRequest(Action onBundlesLoaded, Action onBundlesStartLoading)
        {
            yield return StartCoroutine(RequestBundlesSize(onBundlesStartLoading));
            foreach (var assetBundle in _assetBundlesList)
            {
                _unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(assetBundle.BundleUrl, assetBundle.Hash128);
                UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = _unityWebRequest.SendWebRequest();
                StartCoroutine(CalculateAverageSpeedPerSecond(_unityWebRequest, unityWebRequestAsyncOperation));
                yield return unityWebRequestAsyncOperation;
                Debug.Log(assetBundle.BundleName + " Loaded");
                DownloadHandlerAssetBundle.GetContent(_unityWebRequest);
            }
            onBundlesLoaded?.Invoke();
        }

        private IEnumerator CalculateAverageSpeedPerSecond(UnityWebRequest unityWebRequest,
            UnityWebRequestAsyncOperation unityWebRequestAsyncOperation)
        {
            float time = 0f;
            int previousDownloadedBytes = DownloadedBundleBytes;
            while (!unityWebRequestAsyncOperation.isDone)
            {
                time += Time.deltaTime;
                DownloadedBundleBytes = BundlesSizeInBytes -
                    (BundlesSizeInBytes - (int) unityWebRequest.downloadedBytes) + previousDownloadedBytes;
                if (time >= CalculatingAverageSpeedDelay)
                {
                    time = 0f;
                    _downloadedBytesPerSecond.previos = _downloadedBytesPerSecond.current;
                    _downloadedBytesPerSecond.current = unityWebRequest.downloadedBytes;
                    _downloadedBytesPerSecondList.Add(_downloadedBytesPerSecond.current -
                                                      _downloadedBytesPerSecond.previos);
                    AverageSpeed = _downloadedBytesPerSecondList.Average();
                    RemainDownloadingTime =
                        TimeSpan.FromSeconds((BundlesSizeInBytes - DownloadedBundleBytes) / AverageSpeed);
                }
                yield return null;
            }
        }
    }
}