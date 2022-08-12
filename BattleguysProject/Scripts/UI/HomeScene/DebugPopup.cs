using Monetization;
using UnityEngine;

namespace UI.HomeScene
{
    public class DebugPopup : UiPanel
    {
#pragma warning disable 649
    
#pragma warning restore 649

        #region Constants
        private const int TouchCountToActivatePanel = 5;
        #endregion

        private void Update()
        {
            if ((Input.touchCount >= TouchCountToActivatePanel || Input.GetKeyDown(KeyCode.Escape)) && !IsPanelActive)
            {
                Show();
            }
        }

        public void ShowInterstitialPressed()
        {
            AdsManager.ShowInterstitial();
        }

        public void ShowRewardedPressed()
        {
            AdsManager.ShowRewarded(null);
        }
    }
}