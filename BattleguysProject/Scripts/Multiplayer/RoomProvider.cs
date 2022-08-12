using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Tools;
using UI.HomeScene;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Multiplayer
{
    public class RoomProvider : MonoBehaviourPunCallbacks
    {
#pragma warning disable 649
        [SerializeField] private PhotonManager photonManager;
        [SerializeField] private PasswordPopup passwordPopup;
        [SerializeField] private RoomIsFullPopup roomIsFullPopup;
#pragma warning restore 649

        #region Constants
        public const string TagCustomProperty = "Tag";
        public const string AccessCustomProperty = "Access";
        public const string PasswordCustomProperty = "Password";
        public const string RoundCountCustomProperty = "RoundCount";
        public const string MapCustomProperty = "MapName";
        public const string VictoryConditionCustomProperty = "VictoryCondition";
        public const string SpawnPointId = "SpawnPointId";
        public const string MatchStatisticsPropertyId = "MatchStatistics";
        public const string CurrentRoundPropertyId = "CurrentRound";
        #endregion

        public static event Action<Hashtable> OnRoomPropertiesUpdatedEvent;
        
        public static void CreateRoom(RoomInfo roomInfo)
        {
            if (!PhotonNetwork.IsConnectedAndReady)
                return;
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = roomInfo.MaxPlayers,
                CustomRoomPropertiesForLobby = new []
                {
                    TagCustomProperty, AccessCustomProperty, PasswordCustomProperty, RoundCountCustomProperty, MapCustomProperty,
                    VictoryConditionCustomProperty
                },

                CustomRoomProperties = new Hashtable
                {
                    {MatchStatisticsPropertyId, string.Empty},
                    {CurrentRoundPropertyId, string.Empty},
                    {TagCustomProperty, roomInfo.Tag},
                    {AccessCustomProperty, roomInfo.RoomAccess},
                    {PasswordCustomProperty, roomInfo.Password},
                    {RoundCountCustomProperty, roomInfo.RoundCount},
                    {MapCustomProperty, roomInfo.MapName},
                    {VictoryConditionCustomProperty, roomInfo.VictoryCondition},
                    {SpawnPointId, 0}
                }
            };
            string roomInfoName = roomInfo.Name;
            CheckRoomName(ref roomInfoName);
            PhotonNetwork.CreateRoom(roomInfoName, roomOptions);
        }

        private static void CheckRoomName(ref string roomName)
        {
            if (string.IsNullOrEmpty(roomName) || string.IsNullOrWhiteSpace(roomName))
            {
                roomName = $"Room{Random.Range(0, 100)}";
            }
        }

        public void JoinRoom(RoomInfo uiRoomInfo)
        {
            if (!PhotonNetwork.IsConnectedAndReady)
                return;
            if (uiRoomInfo.PlayerCount == 0)
                return;
            if (uiRoomInfo.PlayerCount == uiRoomInfo.MaxPlayers)
            {
                roomIsFullPopup.Show();
                return;
            }
            if (uiRoomInfo.RoomAccess == RoomAccess.Public)
            {
                PhotonNetwork.JoinRoom(uiRoomInfo.Name);
            }
            else
            {
                passwordPopup.Show();
                passwordPopup.CheckPasswordAction = (enteredPassword) =>
                {
                    if (HashingUtility.GetHash(enteredPassword) == uiRoomInfo.Password)
                    {
                        passwordPopup.Hide();
                        PhotonNetwork.JoinRoom(uiRoomInfo.Name);
                    }
                    else
                    {
                        passwordPopup.ShowErrorMessage();
                    }
                };
            }
        }
        
        public static void UpdateProperties(Hashtable newHashTable)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(newHashTable);
        }

        public static void JoinRandomRoom()
        {
            if (!PhotonNetwork.IsConnectedAndReady)
                return;
            PhotonNetwork.JoinRandomRoom();
        }

        public static void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    
        public override void OnJoinedRoom()
        {
            photonManager.LoadLevel(Constants.DungeonSceneName);
        }

        public override void OnLeftRoom()
        {
            photonManager.LoadLevel(Constants.MenuSceneName);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            OnRoomPropertiesUpdatedEvent?.Invoke(propertiesThatChanged);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom(new RoomInfo($"Room{Random.Range(0, 100)}"));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            LobbyProvider.JoinLobby();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            
        }
    }
}