using System;
using System.Collections;
using AssetBundle;
using Tools;
using UnityEngine;

namespace UI.LoadingScene
{
    public class LoadingAssetBundlesPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private AssetBundleProvider assetBundleProvider;
        [SerializeField] private DownloadingStatistics downloadingStatistics;
        [SerializeField] private LoadingSlider loadingSlider;
        [SerializeField] private LoadingIndicator loadingIndicator;
#pragma warning restore 649

        #region Constants
        private const string LoadingBundlesIndicatorStatus = "Receiving data from server...";
        #endregion

        private Coroutine _updateStatisticsCoroutine;

        private void OnBundlesStartLoading()
        {
            loadingIndicator.Hide();
            Show();
            loadingSlider.SetActive(true);
            loadingSlider.Init(AssetBundleProvider.BundlesSizeInBytes);
            StartUpdateStatistics();
        }

        public void LoadBundles(Action onBundlesLoaded)
        {
            loadingIndicator.ShowAndSetIndicatorStatus(LoadingBundlesIndicatorStatus);
            onBundlesLoaded += OnBundlesLoaded;
            assetBundleProvider.LoadBundles(onBundlesLoaded, OnBundlesStartLoading);
        }

        private void SetStatistics(TimeSpan remainingTime, float downloadingSpeed, float downloadedBytes)
        {
            downloadingStatistics.Set(remainingTime,
                $"{InfoAbbreviationUtility.ConvertByteTo(downloadingSpeed, InfoAbbreviationUtility.InfoAbbreviationEnum.Mb).value:F1} Mb/s");
            var progress =
                InfoAbbreviationUtility.ConvertByteTo(downloadedBytes, InfoAbbreviationUtility.InfoAbbreviationEnum.Mb);
            loadingSlider.SetValue(downloadedBytes,
                $"<color=#00FFFF>Downloading...</color>{progress.value:F1}{progress.abbreviation} / {AssetBundleProvider.BundlesSizeInMegabytes:F1}Mb");
        }

        private void StartUpdateStatistics()
        {
            _updateStatisticsCoroutine ??= StartCoroutine(UpdateStatistics());
        }

        private IEnumerator UpdateStatistics()
        {
            while (true)
            {
                SetStatistics(AssetBundleProvider.RemainDownloadingTime, AssetBundleProvider.AverageSpeed,
                    AssetBundleProvider.DownloadedBundleBytes);
                yield return null;
            }
        }

        private void OnBundlesLoaded()
        {
            loadingIndicator.Hide();
            Hide();
            if (_updateStatisticsCoroutine != null)
            {
                StopCoroutine(_updateStatisticsCoroutine);
                _updateStatisticsCoroutine = null;
            }
        }
    }
}