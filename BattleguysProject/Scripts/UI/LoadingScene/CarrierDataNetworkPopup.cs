using AssetBundle;
using TMPro;
using UnityEngine;
using Network = AssetBundle.Network;

namespace UI.LoadingScene
{
    public class CarrierDataNetworkPopup : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI fileInfoTextMeshProUGUI;
#pragma warning restore 649

        public override void Show()
        {
            fileInfoTextMeshProUGUI.text = $"Assets size is {AssetBundleProvider.BundlesSizeInMegabytes:F1}Mb";
            base.Show();
        }

        public void ApprovePressed()
        {
            Hide();
            Network.RequestCarrierDataNetworkResult(true);
        }

        public void RejectPressed()
        {
            Hide();
            Network.RequestCarrierDataNetworkResult(false);
            Application.Quit();
        }
    }
}