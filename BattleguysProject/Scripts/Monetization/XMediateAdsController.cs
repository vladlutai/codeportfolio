using System;
using XMediateAds.Api;

namespace Monetization
{
    public class XMediateAdsController : AdsController
    {
        [Serializable]
        public class CredentialKeys
        {
            public string androidPublisher;
            public string iosPublisher;
            public string androidApplication;
            public string iosApplication;
        }
        
        private BannerView _bannerView;
        private FullscreenAdView _fullscreenView;
        private RwdVideoView _rwdVideoView;

        public XMediateAdsController()
        {
            Initialize();
        }

        protected sealed override void Initialize()
        {
#if UNITY_EDITOR
        string pubId = "YOUR-EDITOR-PUBLISHER-ID";
        string appId = "YOUR-EDITOR-APPLICATION-ID";
        IsRewardedLoaded = true;
        IsInterstitialLoaded = true;
        return;
#elif UNITY_ANDROID
		string pubId = "1285c801-5fee-4d77-a389-128608923cab";
		string appId = "946f45da-8286-48ca-8aba-35abcc6a2b31";
#elif UNITY_IOS
		string pubId = "1285c801-5fee-4d77-a389-128608923cab";
		string appId = "98a54137-ef9b-45cb-ad3b-67532b070a14";
#endif
        XMediate.updateGDPRSettings(true, true);
        XMediate.init(pubId, appId);
        InitBannerAd();
        InitFullscreenAd();
        InitRewardedAd();
        RequestInterstitial();
        RequestRewarded();
        }

        public override void ShowBanner()
        {
        
        }

        protected override void InitializeBanner()
        {
            _bannerView = XMediate.createBannerAdView(XmBannerType.BANNER, AdPosition.Bottom);
            _bannerView.OnBannerLoaded += HandleBannerLoaded;
            _bannerView.OnBannerFailedToLoad += HandleBannerFailedToLoad;
            _bannerView.OnBannerClicked += HandleBannerClicked;
        }

        protected override void InitializeInterstitial()
        {
            _fullscreenView = XMediate.createFullscreenAdView();
            _fullscreenView.OnFullscreenAdLoaded += HandleInterstitialLoaded;
            _fullscreenView.OnFullscreenAdFailedToLoad += HandleInterstitialFailedToLoad;
            _fullscreenView.OnFullscreenAdShown += HandleInterstitialShown;
            _fullscreenView.OnFullscreenAdFailedToPlay += HandleInterstitialFailedToPlay;
            _fullscreenView.OnFullscreenAdClicked += HandleInterstitialClicked;
            _fullscreenView.OnFullscreenAdDismissed += HandleInterstitialDismissed;
        }

        protected override void InitializeRewarded()
        {
            _rwdVideoView = XMediate.createRewardedAdView();
            _rwdVideoView.OnRwdVideoLoaded += HandleRewardedLoaded;
            _rwdVideoView.OnRwdVideoFailedToLoad += HandleRewardedFailedToLoad;
            _rwdVideoView.OnRwdVideoOpened += HandleRewardedOpened;
            _rwdVideoView.OnRwdVideoFailedToPlay += HandleRewardedFailedToPlay;
            _rwdVideoView.OnRwdVideoClicked += HandleRewardedClicked;
            _rwdVideoView.OnRwdVideoClosed += HandleRewardedClosed;
        }

        public override void ShowInterstitial()
        {
            _fullscreenView.Show();
        }

        public override void ShowRewarded(Action onSuccessAction)
        {
            _rwdVideoView.Show();
            RewardedSuccessAction = onSuccessAction;
        }
    
        protected override void RequestBanner()
        {
            _bannerView.LoadAd(null);
        }    
    
        protected override void RequestInterstitial()
        {
            IsInterstitialLoaded = false;
            _fullscreenView.LoadAd(null);
        }

        protected override void RequestRewarded()
        {
            IsRewardedLoaded = false;
            _rwdVideoView.LoadAd(null);
        }   

        #region BannerHandles
        protected override void HandleBannerLoaded()
        {
       
        }

        protected override void HandleBannerFailedToLoad(string errorCode)
        {
            RequestBanner();
        }

        protected override void HandleBannerClicked()
        {
        
        }
        #endregion

        #region HandleFullScreen
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
            EventManager.SendEvent(Constants.AdsIsShowing);
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
            EventManager.SendEvent(Constants.AdsIsClosed);
            RequestInterstitial();
        }
        #endregion

        #region HandleRewarded
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
            EventManager.SendEvent(Constants.AdsIsShowing);
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
            EventManager.SendEvent(Constants.AdsIsClosed);
            RewardedSuccessAction?.Invoke();
            RequestRewarded();
        }
        #endregion
    }
}