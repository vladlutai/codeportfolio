using System;
using RemoteConfig;
using UnityEngine;

namespace Monetization
{
    public class AppLovinAdsController : AdsController
    {
        [Serializable]
        public class CredentialKeys
        {
            public string sdk;
            public string androidBanner;
            public string iosBanner;
            public string androidInterstitial;
            public string iosInterstitial;
            public string androidRewarded;
            public string iosRewarded;
        }
        
        #region Constants
        #endregion

        private readonly string _sdkKey;
        private readonly string _bannerAdUnitId;
        private readonly string _interstitialAdUnitId;
        private readonly string _rewardedAdUnitId;

        public AppLovinAdsController()
        {
            CredentialKeys credentialKeys = RemoteConfigController.MonetizationConfig.applovinCredentialKeys;
            _sdkKey = credentialKeys.sdk;
#if UNITY_ANDROID
            _bannerAdUnitId = credentialKeys.androidBanner;
            _interstitialAdUnitId = credentialKeys.androidInterstitial;
            _rewardedAdUnitId = credentialKeys.androidRewarded;
#elif UNITY_IOS
            _bannerAdUnitId = credentialKeys.iosBanner;
            _interstitialAdUnitId = credentialKeys.iosInterstitial;
            _rewardedAdUnitId = credentialKeys.iosRewarded;
#endif
            Initialize();
        }

        #region ShowRequests
        public override void ShowBanner()
        {
            MaxSdk.ShowBanner(_bannerAdUnitId);
        }

        public override void ShowInterstitial()
        {
            if (IsInterstitialLoaded)
                MaxSdk.ShowInterstitial(_interstitialAdUnitId);
            {
                RequestInterstitial();
            }
        }

        public override void ShowRewarded(Action onSuccessAction)
        {
            if (IsRewardedLoaded)
            {
                RewardedSuccessAction = onSuccessAction;
                MaxSdk.ShowRewardedAd(_rewardedAdUnitId);
            }
            else
            {
                RequestRewarded();
            }
        }
        #endregion

        #region LoadRequests
        protected override void RequestBanner()
        {
            
        }

        protected override void RequestInterstitial()
        {
            IsInterstitialLoaded = false;
            MaxSdk.LoadInterstitial(_interstitialAdUnitId);
        }

        protected override void RequestRewarded()
        {
            IsRewardedLoaded = false;
            MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
        }
        #endregion

        #region InitializeHandlers
        protected sealed override void Initialize()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
                //InitializeBanner();
                InitializeInterstitial();
                InitializeRewarded();
            };

            MaxSdk.SetSdkKey(_sdkKey);
            MaxSdk.InitializeSdk();
        }

        protected override void InitializeBanner()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += HandleBannerLoaded;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += HandleBannerFailedToLoad;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += HandleBannerClicked;

            MaxSdk.CreateBanner(_bannerAdUnitId, MaxSdkBase.BannerPosition.TopCenter);
            MaxSdk.SetBannerBackgroundColor(_bannerAdUnitId, Color.white);
        }

        protected override void InitializeInterstitial()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            
            RequestInterstitial();
        }

        protected override void InitializeRewarded()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            RequestRewarded();
        }

        #endregion

        #region BannerHandlers
        protected override void HandleBannerLoaded()
        {
            
        }
        
        protected override void HandleBannerFailedToLoad(string errorCode)
        {
            
        }
        
        protected override void HandleBannerClicked()
        {
            
        }

        #region AppLovinCallbacks
        private void HandleBannerLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            HandleBannerLoaded();
        }
        
        private void HandleBannerFailedToLoad(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            HandleBannerFailedToLoad(errorInfo.Message);
        }

        private void HandleBannerClicked(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            HandleBannerClicked();
        }
        #endregion
        #endregion

        #region InterstitialHandlers
        protected override void HandleInterstitialLoaded()
        {
            IsInterstitialLoaded = true;
        }

        protected override void HandleInterstitialFailedToLoad(string errorCode)
        {
            RequestInterstitial();
        }

        protected override void HandleInterstitialShown()
        {
            
        }

        protected override void HandleInterstitialFailedToPlay(string errorCode)
        {
            RequestInterstitial();
        }

        protected override void HandleInterstitialClicked()
        {
            
        }

        protected override void HandleInterstitialDismissed()
        {
            RequestInterstitial();
        }

        #region AppLovinCallbacks
        private void InterstitialFailedToDisplayEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
        {
            HandleInterstitialFailedToPlay(arg2.Message);
        }
        
        private void OnInterstitialFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            HandleInterstitialFailedToLoad(arg2.Message);
        }

        private void OnInterstitialLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            HandleInterstitialLoaded();
        }
        
        private void OnInterstitialDismissedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            HandleInterstitialDismissed();
        }
        #endregion
        #endregion

        #region RewardedHandlers
        protected override void HandleRewardedLoaded()
        {
            IsRewardedLoaded = true;
        }

        protected override void HandleRewardedFailedToLoad(string errorCode)
        {
            RequestRewarded();
        }

        protected override void HandleRewardedOpened()
        {
            
        }

        protected override void HandleRewardedFailedToPlay(string errorCode)
        {
            RequestRewarded();
        }

        protected override void HandleRewardedClicked()
        {
            
        }

        protected override void HandleRewardedClosed()
        {
            RewardedSuccessAction?.Invoke();
            RewardedSuccessAction = null;
            RequestRewarded();
        }

        #region AppLovinCallBacks
        private void OnRewardedAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            HandleRewardedLoaded();
        }
        private void OnRewardedAdDismissedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            HandleRewardedClosed();
        }

        private void OnRewardedAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            HandleRewardedClicked();
        }

        private void OnRewardedAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            
        }

        private void OnRewardedAdFailedToDisplayEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
        {
            HandleRewardedFailedToPlay(arg2.Message);
        }

        private void OnRewardedAdFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            HandleRewardedFailedToLoad(arg2.Message);
        }
        #endregion
        #endregion
    }
}