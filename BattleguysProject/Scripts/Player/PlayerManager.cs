using System;
using System.Collections.Generic;
using Multiplayer;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private PhotonPrefabPool photonPrefabPool;
        [SerializeField] private PlayerCameraSwitcher playerCameraSwitcher;
#pragma warning restore 649

        #region Constants
        #endregion

        public static event Action OnCurrentPlayerSpawnedEvent;
        public static event Action OnCurrentPlayerDiedEvent;
        public static event Action<int> OnPlayerInstantiatedEvent;
        public static event Action <int> OnPlayerDestroyedEvent;
        public static event Action<int, int> OnPlayerDiedEvent;

        public static PlayerController CurrentPlayerController { get; private set; }

        public static readonly Dictionary<int , PlayerController> PlayerControllersDictionary =
            new Dictionary<int, PlayerController>();

        private void Start()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                SpawnPhotonPlayer();
            }
        }

        private void SpawnPhotonPlayer()
        {
            CurrentPlayerController = PhotonNetwork.Instantiate(photonPrefabPool.PlayerPrefabName, Vector3.zero, Quaternion.identity)
                .GetComponent<PlayerController>();
            OnCurrentPlayerSpawnedEvent?.Invoke();
            playerCameraSwitcher.SetVirtualCameraTarget(CurrentPlayerController.CameraRootTransform);
        }

        public static void OnPlayerInstantiated(PlayerController playerController)
        {
            PlayerControllersDictionary[playerController.PhotonView.ViewID] = playerController;
            if (PhotonNetwork.IsMasterClient)
            {
                playerController.Init();
            }
            OnPlayerInstantiatedEvent?.Invoke(playerController.PhotonView.ViewID);
        }

        public static void OnPlayerDestroyed(PlayerController playerController)
        {
            PlayerControllersDictionary.Remove(playerController.PhotonView.ViewID);
            OnPlayerDestroyedEvent?.Invoke(playerController.PhotonView.ViewID);
        }

        public static void OnPlayerDied(PlayerController playerController, PlayerController playerControllerKiller)
        {
            if (playerController == CurrentPlayerController)
            {
                OnCurrentPlayerDiedEvent?.Invoke();
            }
            OnPlayerDiedEvent?.Invoke(playerController.PhotonView.ViewID, playerControllerKiller.PhotonView.ViewID);
        }
    }
}