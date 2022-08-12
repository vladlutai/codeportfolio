using System;
using System.Collections.Generic;
using RemoteConfig;
using Tools;
using UnityEngine;

namespace Monetization
{
    public enum MonetizationProvider
    {
        Xmediate = 0,
        Applovin = 1
    }

    public static class AdsManager
    {
        private static readonly AdsController AdsController;

        #region Constants
        private const MonetizationProvider DefaultMonetizationProvider = MonetizationProvider.Applovin;
        #endregion

        private static readonly Dictionary<MonetizationProvider, Func<AdsController>> Dictionary =
            new Dictionary<MonetizationProvider, Func<AdsController>>
            {
                {MonetizationProvider.Xmediate, () => new XMediateAdsController()},
                {MonetizationProvider.Applovin, () => new AppLovinAdsController()}
            };

        static AdsManager()
        {
            AdsController =
                Dictionary[
                    System.Enum.TryParse(RemoteConfigController.MonetizationConfig.providerId.ToString(),
                        out MonetizationProvider monetizationProvider)
                        ? monetizationProvider
                        : DefaultMonetizationProvider]();
            AdsController.RewardedLoad += RewardedLoadHandler;
        }

        private static void RewardedLoadHandler(bool isLoaded)
        {
            if(AdsController is null)
                return;
            string eventName = isLoaded ? Constants.RewardedAdsAvailable : Constants.RewardedAdsUnAvailable;
            EventManager.SendEvent(eventName);
        }

        public static void ShowInterstitial()
        {
#if UNITY_EDITOR
            Debug.Log("ShowInterstitial");
            return;
#endif
            AdsController?.ShowInterstitial();
        }

        public static void ShowRewarded(Action onSuccessAction)
        {
#if UNITY_EDITOR
            Debug.Log("ShowRewarded Success");
            onSuccessAction?.Invoke();
            return;
#endif
            AdsController?.ShowRewarded(onSuccessAction);
        }
    }
}