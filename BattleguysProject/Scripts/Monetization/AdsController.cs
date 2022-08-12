using System;

namespace Monetization
{
    public abstract class AdsController
    {
        private bool _isRewardedLoaded;
        protected Action RewardedSuccessAction = null;
        public Action<bool> RewardedLoad = null;
        public bool IsInterstitialLoaded { get; protected set; }

        protected bool IsRewardedLoaded
        {
            set
            {
                _isRewardedLoaded = value;
                RewardedLoad?.Invoke(_isRewardedLoaded);
            }
            get => _isRewardedLoaded;
        }

        #region InitializeHandlers
        protected abstract void Initialize();
        protected abstract void InitializeBanner();
        protected abstract void InitializeInterstitial();
        protected abstract void InitializeRewarded();
        #endregion

        #region ShowRequests
        public abstract void ShowBanner();
        public abstract void ShowInterstitial();
        public abstract void ShowRewarded(Action onSuccessAction);
        #endregion

        #region LoadRequests
        protected abstract void RequestBanner();
        protected abstract void RequestInterstitial();
        protected abstract void RequestRewarded();
        #endregion

        #region BannerHandles
        protected abstract void HandleBannerLoaded();
        protected abstract void HandleBannerFailedToLoad(string errorCode);
        protected abstract void HandleBannerClicked();
        #endregion

        #region InterstitialHandles
        protected abstract void HandleInterstitialLoaded();
        protected abstract void HandleInterstitialFailedToLoad(string errorCode);
        protected abstract void HandleInterstitialShown();
        protected abstract void HandleInterstitialFailedToPlay(string errorCode);
        protected abstract void HandleInterstitialClicked();
        protected abstract void HandleInterstitialDismissed();
        #endregion

        #region RewardedHandles
        protected abstract void HandleRewardedLoaded();
        protected abstract void HandleRewardedFailedToLoad(string errorCode);
        protected abstract void HandleRewardedOpened();
        protected abstract void HandleRewardedFailedToPlay(string errorCode);
        protected abstract void HandleRewardedClicked();
        protected abstract void HandleRewardedClosed();
        #endregion
    }
}