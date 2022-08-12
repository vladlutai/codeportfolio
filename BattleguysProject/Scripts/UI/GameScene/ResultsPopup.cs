using Multiplayer;
using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class ResultsPopup : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI descriptionTextMeshPro;
#pragma warning restore 649

        public void HomeButtonPressed()
        {
            Hide();
            RoomProvider.LeaveRoom();
        }
    }
}