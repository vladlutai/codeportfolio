using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using RemoteConfig;
using TMPro;
using UI.HomeScene;
using UnityEngine;
using User;

namespace Multiplayer
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
#pragma warning disable 649
        [SerializeField] private LoadingPanel loadingPanel;
        [SerializeField] private TextMeshProUGUI debugTextMeshPro;
#pragma warning restore 649

        #region Constants
        private const string GameVersion = "1";
        #endregion

        private Action<bool> _connectionResultAction;
        private ClientState _clientState;

        private void Awake()
        {
            _clientState = PhotonNetwork.NetworkClientState;
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.GameVersion = GameVersion;
            LogState(_clientState.ToString());
        }

        private new void OnEnable()
        {
            base.OnEnable();
        }

        private new void OnDisable()
        {
            base.OnDisable();
        }
        
        private void Update()
        {
            if (_clientState != PhotonNetwork.NetworkClientState)
            {
                _clientState = PhotonNetwork.NetworkClientState;
                LogState(_clientState.ToString());
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!PhotonNetwork.IsConnectedAndReady)
                {
                    ConnectToServer();
                }
                else
                {
                    Disconnect();
                }
            }
#endif
        }

        private void LogState(string state)
        {
            if(debugTextMeshPro == null)
                return;
            debugTextMeshPro.text = state;
        }

        public void ConnectToServer(Action<bool> connectionResultAction = null)
        {
            LogState("FetchData");
            _connectionResultAction = connectionResultAction;
            RemoteConfigController.FetchConfig(result =>
            {
                if (result)
                {
                    LogState("ConnectingToPhoton");
                    PhotonNetwork.NickName = UserInfo.Nickname;
                    PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = RemoteConfigController.AvailablePhotonAppIds.photonAppIds[0];
                    PhotonNetwork.ConnectUsingSettings();
                }
                else
                {
                    _connectionResultAction?.Invoke(false);
                    LogState("Can't connect to server");
                }
            });
        }

        private void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void LoadLevel(string sceneName)
        {
            LoadScene(sceneName);
        }

        private void LoadScene(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
            StartCoroutine(UpdateLoadingProgress());
        }

        private IEnumerator UpdateLoadingProgress()
        {
            loadingPanel.InitLoadingSlider(100);
            while (PhotonNetwork.LevelLoadingProgress <= 0.9f)
            {
                loadingPanel.UpdateLoadingSliderValue((PhotonNetwork.LevelLoadingProgress / 0.9f) * 100f);
                yield return null;
            }
        }
        
        public override void OnConnectedToMaster()
        {
            _connectionResultAction?.Invoke(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _connectionResultAction?.Invoke(false);
        }
    }
}