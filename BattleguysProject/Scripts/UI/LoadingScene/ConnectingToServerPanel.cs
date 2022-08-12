using System;
using Multiplayer;
using RemoteConfig;
using UnityEngine;

namespace UI.LoadingScene
{
    public class ConnectingToServerPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private PhotonManager photonManager;
#pragma warning restore 649

        public void ConnectToServer(Action<bool> connectionResultAction)
        {
            Show();
            photonManager.ConnectToServer(connectionResultAction);
        }
    }
}