using System;
using Unity.RemoteConfig;
using UnityEngine;

namespace RemoteConfig
{
    public struct UserAttributes
    {
    }

    public struct AppAttributes
    {
    }

    public static class RemoteConfigController
    {
        private static Data.AssetBundleFolderUrlConfig _assetBundleFolderUrlConfig;
        
        #region Constants
        private const string AvailablePhotonAppIdsJsonKey = "AvailablePhotonAppIdsJson";
        private const string AssetBundleFolderUrlConfigJsonKey = "AssetBundleFolderUrlConfigJson";
        private const string AvailableAssetBundlesJsonKey = "AvailableAssetBundlesJson";
        private const string MonetizationConfigJsonKey = "MonetizationConfigJson";
        private const string AnalyticConfigJsonKey = "AnalyticConfigJson";
        private const string MatchConfigJsonKey = "MatchConfigJson";
        #endregion
        
        public static string BundleUrl { get; private set; }
        public static Data.AvailablePhotonAppIds  AvailablePhotonAppIds { get; private set; }
        public static Data.AvailableAssetBundles AvailableAssetBundles { get; private set; }
        public static Data.MonetizationConfig MonetizationConfig { get; private set; }
        public static Data.AnalyticsConfig AnalyticsConfig { get; private set; }

        public static Data.MatchConfig MatchConfig { get; private set; }

        public static void FetchConfig(Action<bool> onFetchCompleted)
        {
            ConfigManager.FetchConfigs(new UserAttributes(), new AppAttributes());
            ConfigManager.FetchCompleted += configResponse =>
            {
                if (configResponse.status == ConfigRequestStatus.Success)
                {
                    _assetBundleFolderUrlConfig =
                        SerializeUtility.SerializeUtility.DeserializeJSon<Data.AssetBundleFolderUrlConfig>(
                            ConfigManager.appConfig.GetJson(AssetBundleFolderUrlConfigJsonKey));
                    BundleUrl = _assetBundleFolderUrlConfig.GetUrlByRuntimePlatform(Application.platform);
                    AvailablePhotonAppIds = SerializeUtility.SerializeUtility.DeserializeJSon<Data.AvailablePhotonAppIds>(
                        ConfigManager.appConfig.GetJson(AvailablePhotonAppIdsJsonKey));
                    AvailableAssetBundles =
                        SerializeUtility.SerializeUtility.DeserializeJSon<Data.AvailableAssetBundles>(
                            ConfigManager.appConfig.GetJson(AvailableAssetBundlesJsonKey));
                    MonetizationConfig =
                        SerializeUtility.SerializeUtility.DeserializeJSon<Data.MonetizationConfig>(
                            ConfigManager.appConfig.GetJson(MonetizationConfigJsonKey));
                    AnalyticsConfig =
                        SerializeUtility.SerializeUtility.DeserializeJSon<Data.AnalyticsConfig>(
                            ConfigManager.appConfig.GetJson(AnalyticConfigJsonKey));
                    MatchConfig =
                        SerializeUtility.SerializeUtility.DeserializeJSon<Data.MatchConfig>(
                            ConfigManager.appConfig.GetJson(MatchConfigJsonKey));
                    onFetchCompleted?.Invoke(true);
                }
                else
                {
                    onFetchCompleted?.Invoke(false);
                }
            };
        }
    }
}