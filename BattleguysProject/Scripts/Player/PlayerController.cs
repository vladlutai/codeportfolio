using System.Collections.Generic;
using MatchMaking;
using Photon.Pun;
using Tools;
using UnityEngine;
using UI.HomeScene.HeroStats;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerController : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
#pragma warning disable 649
        [SerializeField] private PlayerInfo playerInfo;
        [SerializeField] private Transform cameraRootTransform;
#pragma warning restore 649

        #region Constants
        private const int PlayerLayer = 6;
        #endregion

        public PhotonView PhotonView => photonView;
        public Transform CameraRootTransform => cameraRootTransform;
        public MatchProvider.TeamEnum TeamEnum => playerInfo.TeamEnum;
        public PlayerInfo PlayerInfo => playerInfo;
        private PlayerController _playerControllerKiller;
        private readonly List<PlayerController> _triggeredPlayerControllersList = new List<PlayerController>();

        private new void OnEnable()
        {
            base.OnEnable();
            if (photonView.IsMine)
            {
                EventManager.StartListening(Constants.SetPlayersToSpawnPoints, SetPlayerToSpawnPoint);
            }
        }

        private void Awake()
        {
            playerInfo.OnDeath += OnDeath;
            playerInfo.OnRevive += OnRevive;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            if (photonView.IsMine)
            {
                EventManager.StopListening(Constants.SetPlayersToSpawnPoints, SetPlayerToSpawnPoint);
            }
        }

        private void OnDestroy()
        {
            PlayerManager.OnPlayerDestroyed(this);
        }

        public void Init()
        {
            playerInfo.Init();
        }

        private void OnDeath()
        {
            PlayerManager.OnPlayerDied(this, _playerControllerKiller);
        }

        public void Revive()
        {
            _triggeredPlayerControllersList.Clear();
            playerInfo.Revive();
            UpdatePhotonProperties();
        }

        private void OnRevive()
        {
            SetPlayerToSpawnPoint();
        }

        private void SetPlayerToSpawnPoint()
        {
            transform.SetPositionAndRotation(playerInfo.SpawnPoint.transform.position,
                playerInfo.SpawnPoint.transform.rotation);
        }

        #region PhotonCallbacks
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (photonView.Controller.CustomProperties.Count > 0)
                playerInfo.OnPropertiesUpdate(photonView.Controller.CustomProperties);
            PlayerManager.OnPlayerInstantiated(this);
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                //thirdPersonController.SpeedMultiplier = playerInfo.HeroStats[(int) HeroStatType.Speed].PercentValue;
            }
            if (photonView.IsMine || PhotonNetwork.IsMasterClient)
                UpdatePhotonProperties();
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (!photonView.Controller.Equals(targetPlayer))
                return;
            playerInfo.OnPropertiesUpdate(changedProps);
        }
        #endregion

        private void UpdatePhotonProperties()
        {
            photonView.Controller.SetCustomProperties(playerInfo.PropertiesHashtable);
        }

        #region Attack
        [PunRPC]
        private void Attack(float damage)
        {
            foreach (var playerController in _triggeredPlayerControllersList)
            {
                playerController.ReceiveDamage(damage, this);
            }
        }

        private void ReceiveDamage(float damage, PlayerController playerControllerKiller)
        {
            if (playerInfo.IsDead)
                return;
            float actualDamage = damage - damage * playerInfo.HeroStats[(int) HeroStatType.Defence].PercentValue;
            _playerControllerKiller = playerControllerKiller;
            playerInfo.ReceiveDamage(actualDamage);
        }

        private (float damage, bool isCritical) CalculateDamage()
        {
            float critical = Random.value;
            var result = (damage: 0f, isCritical: false);
            if (critical <= playerInfo.HeroStats[(int) HeroStatType.CriticalRate].PercentValue)
            {
                result.damage = playerInfo.HeroStats[(int) HeroStatType.Attack].Value +
                                playerInfo.HeroStats[(int) HeroStatType.CriticalAttack].PercentValue *
                                playerInfo.HeroStats[(int) HeroStatType.Attack].Value;
                result.isCritical = true;
            }
            else
            {
                result.damage = playerInfo.HeroStats[(int) HeroStatType.Attack].Value;
                result.isCritical = false;
            }

            return result;
        }
        #endregion

        #region TriggerHandler
        private void OnTriggerEnter(Collider other)
        {
            OnTrigger(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTrigger(other, false);
        }

        private void OnTrigger(Collider trigger, bool state)
        {
            if (trigger.gameObject.layer != PlayerLayer)
                return;
            PlayerController playerController = trigger.GetComponent<PlayerController>();
            if (playerController == null)
                return;
            if (playerController.Equals(this))
                return;
            if (state)
            {
                if (!_triggeredPlayerControllersList.Contains(playerController))
                {
                    _triggeredPlayerControllersList.Add(playerController);
                }
            }
            else
            {
                if (_triggeredPlayerControllersList.Contains(playerController))
                {
                    _triggeredPlayerControllersList.Remove(playerController);
                }
            }
        }
        #endregion
    }
}