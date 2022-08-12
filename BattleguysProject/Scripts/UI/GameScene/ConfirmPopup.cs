using Multiplayer;

namespace UI.GameScene
{
    public class ConfirmPopup : UiPanel
    {
#pragma warning disable 649
        
#pragma warning restore 649

        public void HomeButtonPressed()
        {
            Hide();
            RoomProvider.LeaveRoom();
        }
    }
}