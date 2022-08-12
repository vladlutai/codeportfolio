using System;
using System.Collections.Generic;
using Photon.Pun;

namespace Multiplayer
{
    public class LobbyProvider : MonoBehaviourPunCallbacks
    {
#pragma warning disable 649
#pragma warning restore 649
        
        public static event Action<List<RoomInfo>> OnRoomListUpdateEvent;
        public static bool InLobby => PhotonNetwork.InLobby;

        public static void JoinLobby()
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.ConnectUsingSettings();
                return;
            }
            if (!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();
        }

        public static void LeaveLobby()
        {
            if(!PhotonNetwork.IsConnectedAndReady)
                return;
            if (PhotonNetwork.InLobby)
                PhotonNetwork.LeaveLobby();
        }

        public override void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
        {
            List<RoomInfo> roomInfos = new List<RoomInfo>();
            foreach (var room in roomList)
            {
                if (room.PlayerCount > 0)
                    roomInfos.Add(new RoomInfo(room));
            }
            OnRoomListUpdateEvent?.Invoke(roomInfos);
        }
    }
}