using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

namespace UI.HomeScene
{
    public class ArenaLobbyPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private RoomProvider roomProvider;
        [SerializeField] private UiRoomInfo uiRoomInfoPrefab;
        [SerializeField] private Transform uiRoomsListParent;
#pragma warning restore 649

        #region Constants
        private const float JoinLobbyTryDelay = 1.5f;
        #endregion

        private Coroutine _joinLobby;

        private readonly List<UiRoomInfo> _uiRoomInfosList = new List<UiRoomInfo>();

        private void OnEnable()
        {
            LobbyProvider.OnRoomListUpdateEvent += UpdateRoomList;
        }

        private void OnDisable()
        {
            LobbyProvider.OnRoomListUpdateEvent -= UpdateRoomList;
        }

        public override void Show()
        {
            base.Show();
            _joinLobby ??= StartCoroutine(JoinLobby());
        }

        public override void Hide()
        {
            base.Hide();
            if (_joinLobby != null)
            {
                StopCoroutine(_joinLobby);
                _joinLobby = null;
            }
            LobbyProvider.LeaveLobby();
        }

        private IEnumerator JoinLobby()
        {
            while (!LobbyProvider.InLobby)
            {
                LobbyProvider.JoinLobby();
                yield return new WaitForSecondsRealtime(JoinLobbyTryDelay);
            }
            _joinLobby = null;
        }

        public void QuickStartPressed()
        {
            RoomProvider.JoinRandomRoom();
        }

        private void UpdateRoomList(List<RoomInfo> roomInfos)
        {
            foreach (var uiRoomInfo in _uiRoomInfosList)
            {
                Destroy(uiRoomInfo.gameObject);
            }

            _uiRoomInfosList.Clear();

            foreach (var roomInfo in roomInfos)
            {
                if (roomInfo.PlayerCount > 0)
                    AddRoom(roomInfo);
            }
        }

        private void AddRoom(RoomInfo roomInfo)
        {
            UiRoomInfo uiRoom = Instantiate(uiRoomInfoPrefab, uiRoomsListParent);
            uiRoom.Set(roomInfo.Name, roomInfo.Tag,
                roomInfo.PlayerCount, roomInfo.MaxPlayers, roomInfo.VictoryCondition, roomInfo.RoomAccess,
                () => roomProvider.JoinRoom(roomInfo));
            _uiRoomInfosList.Add(uiRoom);
        }
    }
}