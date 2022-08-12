using System;
using RemoteConfig;
using Tools;
using UnityEngine;

namespace AssetBundle
{
    public class AssetBundle
    {
        public bool IsCached { get; set; }
        public Hash128 Hash128 { get; set; }
        public double SizeInMegabytes { get; private set; }

        public int SizeInBytes
        {
            get => _sizeInBytes;
            set
            {
                _sizeInBytes = value;
                SizeInMegabytes = Math.Round(InfoAbbreviationUtility
                        .ConvertByteTo(_sizeInBytes, InfoAbbreviationUtility.InfoAbbreviationEnum.Mb).value,
                    SizeRoundSignCount);
            }
        }

        public readonly string BundleName;
        public readonly string BundleUrl;
        public readonly string BundleManifestUrl;

        private int _sizeInBytes;

        #region Constants
        private const string ManifestExtension = ".manifest";
        private const int SizeRoundSignCount = 1;
        #endregion

        public AssetBundle(string bundleName)
        {
            BundleName = bundleName;
            BundleUrl = RemoteConfigController.BundleUrl + BundleName;
            BundleManifestUrl = BundleUrl + ManifestExtension;
        }
    }
}