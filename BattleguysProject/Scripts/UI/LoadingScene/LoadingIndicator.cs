using TMPro;
using UnityEngine;

namespace UI.LoadingScene
{
    public class LoadingIndicator : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI indicatorStatusTextMeshPro;
#pragma warning restore 649

        public void ShowAndSetIndicatorStatus(string status)
        {
            indicatorStatusTextMeshPro.text = status;
            Show();
        }
    }
}