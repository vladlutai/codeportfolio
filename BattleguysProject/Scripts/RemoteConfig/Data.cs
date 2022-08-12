using System;
using System.Collections.Generic;
using Analytics;
using Monetization;
using UnityEngine;

namespace RemoteConfig
{
    public abstract class Data
    {
        [Serializable]
        public class AvailableAssetBundles
        {
            public List<string> assetBundlesNames = new List<string>();
        }
        
        [Serializable]
        public class AvailablePhotonAppIds
        {
            public List<string> photonAppIds = new List<string>();
        }
        
        [Serializable]
        public class MonetizationConfig
        {
            public bool isAdsEnabled = true;
            public int providerId;
            public XMediateAdsController.CredentialKeys xmediateCredentialKeys;
            public AppLovinAdsController.CredentialKeys applovinCredentialKeys;
        }
        
        [Serializable]
        public class AnalyticsConfig
        {
            public AnalyticsController.CredentialKeys credentialKeys;
        }
        
        [Serializable]
        public class AssetBundleFolderUrlConfig
        {
            public string android;
            public string ios;
            public string windows;
            public string mac;

            public string GetUrlByRuntimePlatform(RuntimePlatform runtimePlatform)
            {
                Dictionary<RuntimePlatform, string> urlPlatformDictionary =
                    new Dictionary<RuntimePlatform, string>
                    {
                        {RuntimePlatform.Android, android},
                        {RuntimePlatform.IPhonePlayer, ios},
                        {RuntimePlatform.WindowsPlayer, windows},
                        {RuntimePlatform.WindowsEditor, windows},
                        {RuntimePlatform.OSXPlayer, mac},
                        {RuntimePlatform.OSXEditor, mac}
                    };
                return urlPlatformDictionary.ContainsKey(runtimePlatform)
                    ? urlPlatformDictionary[runtimePlatform]
                    : string.Empty;
            }
        }

        [Serializable]
        public class MatchConfig
        {
            public string waitingOtherPlayersTime;
            public string matchTime;

            public TimeSpan WaitingOtherPlayersTimeSpan
            {
                get
                {
                    if (_waitingOtherPlayersTimeSpan == default)
                    {
                        _waitingOtherPlayersTimeSpan = TimeSpan.Parse(waitingOtherPlayersTime);
                    }
                    return _waitingOtherPlayersTimeSpan;
                }
            }

            public TimeSpan MatchTimeSpan
            {
                get
                {
                    if (_matchTimeSpan == default)
                    {
                        _matchTimeSpan = TimeSpan.Parse(matchTime);
                    }

                    return _matchTimeSpan;
                }
            }

            private TimeSpan _waitingOtherPlayersTimeSpan;
            private TimeSpan _matchTimeSpan;
        }
    }
}