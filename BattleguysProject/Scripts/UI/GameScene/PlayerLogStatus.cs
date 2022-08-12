using Photon.Pun;
using UnityEngine;

namespace UI.GameScene
{
    public class PlayerLogStatus : MonoBehaviourPunCallbacks
    {
        private enum PlayerLogMessageType
        {
            Connected,
            Disconnected
        }
        
#pragma warning disable 649
        [SerializeField] private PlayerLogFactory playerLogFactory;
#pragma warning restore 649

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            AddMessageToLog(PlayerLogMessageType.Connected, newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            AddMessageToLog(PlayerLogMessageType.Disconnected, otherPlayer.NickName);
        }

        private void AddMessageToLog(PlayerLogMessageType playerLogMessageType, string nickname)
        {
            
            DecreaseFadeTimerForOldMessages();
            LogMessage logMessage = playerLogFactory.Pull();
            switch (playerLogMessageType)
            {
                case PlayerLogMessageType.Connected:
                    logMessage.Set($"<color=green>{nickname}</color> connected");
                    break;
                
                case PlayerLogMessageType.Disconnected:
                    logMessage.Set($"<color=green>{nickname}</color> disconnected");
                    break;
            }
        }

        private void DecreaseFadeTimerForOldMessages()
        {
            foreach (var logMessage in playerLogFactory.LogMessagesList)
            {
                logMessage.DecreaseFadeTimer();
            }
        }
    }
}