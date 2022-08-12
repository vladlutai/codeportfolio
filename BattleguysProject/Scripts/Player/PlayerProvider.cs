using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerProvider : MonoBehaviourPunCallbacks
    {
#pragma warning disable 649
        [SerializeField] private List<Transform> spawnPointsTransformList;
#pragma warning restore 649
        
        #region Constants
        private const string PlayerPrefabName = "Player";
        #endregion

        private void Start()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                SpawnPlayer();
            }
        }

        private void SpawnPlayer()
        {
            PlayerController player = PhotonNetwork.Instantiate(PlayerPrefabName,
                Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
            PhotonNetwork.LocalPlayer.CustomProperties["SpawnPoint"] = new Vector3(1f,1f,1f);
        }
    }
}