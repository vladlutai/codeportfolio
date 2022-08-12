using UI.LoadingScene;
using UnityEngine;

namespace AssetBundle
{
    public class Network : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private CarrierDataNetworkPopup carrierDataNetworkPopup;
#pragma warning restore 649
        
        public static bool IsDownloadingApproved { get; private set; } = false;
        
        public static void RequestCarrierDataNetworkResult(bool result)
        {
            IsDownloadingApproved = result;
        }

        public void RequestDownloadData()
        {
            NetworkReachability networkReachability = Application.internetReachability;
            switch (networkReachability)
            {
                case NetworkReachability.NotReachable:
                    break;
                
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    carrierDataNetworkPopup.Show();
                    break;
                
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    IsDownloadingApproved = true;
                    break;
            }
        }
    }
}