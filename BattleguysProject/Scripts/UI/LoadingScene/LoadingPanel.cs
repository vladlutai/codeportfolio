using System.Collections;
using AssetBundle;
using DG.Tweening;
using Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.LoadingScene
{
    public class LoadingPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private Transform backgroundTransform;
        [SerializeField] private LoadingSlider loadingSlider;
        [SerializeField] private LoadingIndicator loadingIndicator;
        [SerializeField] private NetworkErrorPopup networkErrorPopup;
        [SerializeField] private CanvasGroup startButtonCanvasGroup;
        [SerializeField] private LoadingAssetBundlesPanel loadingAssetBundlesPanel;
        [SerializeField] private PhotonManager photonManager;
#pragma warning restore 649

        #region Constants
        private const string ConnectingToServerIndicatorStatus = "Connecting to server...";
        #endregion

        private AsyncOperation _asyncOperation;
        private Coroutine _loadingSceneCoroutine;

        private void Awake()
        {
            titleCanvasGroup.DOFade(0f, 0f);
            backgroundTransform.DOScale(1.05f, 0f);
            startButtonCanvasGroup.DOFade(0f, 0f);
            startButtonCanvasGroup.gameObject.SetActive(false);
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            titleCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutCubic);
        
            yield return new WaitForSeconds(0.3f);
            backgroundTransform.transform.DOKill();
            backgroundTransform.DOScale(1f, 1.5f).SetEase(Ease.Linear);
        
            yield return new WaitForSeconds(1.5f);
            loadingIndicator.ShowAndSetIndicatorStatus(ConnectingToServerIndicatorStatus);
            photonManager.ConnectToServer(OnConnectedToServerHandler);
        }

        private void OnConnectedToServerHandler(bool isDone)
        {
            if (isDone)
            {
                loadingAssetBundlesPanel.LoadBundles(LoadHomeScene);
            }
            else
            {
                networkErrorPopup.Show();
            }
        }

        private void LoadHomeScene()
        {
            _loadingSceneCoroutine ??= StartCoroutine(LoadHomeSceneAsync());
        }

        private IEnumerator LoadHomeSceneAsync()
        {
            loadingSlider.SetActive(true);
            loadingSlider.Init(100);
            _asyncOperation = SceneManager.LoadSceneAsync(Constants.MenuSceneName);
            _asyncOperation.allowSceneActivation = false;
            while (!_asyncOperation.isDone)
            {
                float progress = _asyncOperation.progress / 0.9f * 100f;
                loadingSlider.SetValue(progress, $"<color=#00FFFF>Loading...</color>{progress:F0}%");
                if (_asyncOperation.progress >= 0.9f)
                    break;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            _loadingSceneCoroutine = null;
            loadingSlider.SetActive(false);
            startButtonCanvasGroup.gameObject.SetActive(true);
            startButtonCanvasGroup.DOFade(1f, 1.2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    
        public void StartHomeScenePressed()
        {
            startButtonCanvasGroup.DOKill();
            startButtonCanvasGroup.DOFade(0f, 0f);
            startButtonCanvasGroup.DOFade(1f, 0.2f).SetEase(Ease.Linear).SetLoops(6, LoopType.Yoyo).OnComplete(() =>
            {
                _asyncOperation.allowSceneActivation = true;
            });
        }
    }
}