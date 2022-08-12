using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using Photon.Pun;
using Player;
using RemoteConfig;
using TMPro;
using Tools;
using UI;
using UI.GameScene;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MatchMaking
{
    public class MatchProvider : MonoBehaviourPunCallbacks, IPunObservable
    {
        public enum TeamEnum
        {
            None,
            Team1,
            Team2
        }
        
#pragma warning disable 649
        [SerializeField] private PanelSwitcher panelSwitcher;
        [SerializeField] private GamePanel gamePanel;
        [SerializeField] private MatchStatisticPanel matchStatisticPanel;
        [SerializeField] private GameResultPanel gameResultPanel;
        [SerializeField] private GameObject waitingForOpponentsGameObject;
        [SerializeField] private TextMeshProUGUI timerTextMeshPro;
#pragma warning restore 649

        #region Constants
        private const string TimerTextFormat = @"m\:ss";
        private readonly TimeSpan _defaultWaitingOtherPlayersTimeTimeSpan = new TimeSpan(0,0,1,0);
        private readonly TimeSpan _defaultMatchTimeTimeSpan = new TimeSpan(0,0,2,0);
        private readonly TimeSpan _matchStatisticsActiveTimeSpan = new TimeSpan(0, 0, 0, 5);
        private const int AmountOfRounds = 3;
        #endregion

        private TimeSpan _timer;
        private Coroutine _timerCoroutine;
        private Coroutine _matchEndCoroutine;
        private TimeSpan _waitingOtherPlayersTimeTimeSpan;
        private TimeSpan _matchTimeTimeSpan;
        private static int _currentRound = 0;

        private static readonly List<PlayerController> PlayerControllersTeam1 = new List<PlayerController>();
        private static readonly List<PlayerController> PlayerControllersTeam2 = new List<PlayerController>();

        public static int CurrentRound
        {
            get => _currentRound;
            private set
            {
                _currentRound = value;
                if (PhotonNetwork.IsMasterClient)
                    UpdateRoomProperties();
            }
        }

        private TimeSpan Timer
        {
            get => _timer;
            set
            {
                _timer = value;
                timerTextMeshPro.text = _timer.ToString(TimerTextFormat);
            } 
        }

        private void Start()
        {
            if (RemoteConfigController.MatchConfig != null)
            {
                _waitingOtherPlayersTimeTimeSpan = RemoteConfigController.MatchConfig.WaitingOtherPlayersTimeSpan;
                _matchTimeTimeSpan = RemoteConfigController.MatchConfig.MatchTimeSpan;
            }
            else
            {
                _waitingOtherPlayersTimeTimeSpan = _defaultWaitingOtherPlayersTimeTimeSpan;
                _matchTimeTimeSpan = _defaultMatchTimeTimeSpan;
            }
            if (PhotonNetwork.IsMasterClient && _currentRound != AmountOfRounds)
            {
                WaitForOtherPlayers(DateTime.Now.Add(_waitingOtherPlayersTimeTimeSpan), StartMatch);
            }
        }
        
        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.Equals(newMasterClient) && Timer.TotalMilliseconds > 0 && _currentRound < AmountOfRounds)
            {
                WaitForOtherPlayers(DateTime.Now.Add(Timer), StartMatch);
            }
        }

        private void StartMatch()
        {
            InitMatch();
        }

        private void InitMatch()
        {
            photonView.RPC("OnMatchStarted", RpcTarget.All);
            _timerCoroutine ??= StartCoroutine(StartTimer(DateTime.Now.Add(_matchTimeTimeSpan), OnMatchEnded));
        }

        [PunRPC]
        private void OnMatchStarted()
        {
            panelSwitcher.SwitchPressed(gamePanel);
            if (PhotonNetwork.IsMasterClient)
                CurrentRound++;
            waitingForOpponentsGameObject.SetActive(false);
            EventManager.SendEvent(Constants.MatchStarted);
            EventManager.SendEvent(Constants.SetPlayersToSpawnPoints);
        }

        private void OnMatchEnded()
        {
            photonView.RPC("MatchEndHandler", RpcTarget.All);
        }

        [PunRPC]
        private void MatchEndHandler()
        {
            if (_matchEndCoroutine == null)
                _matchEndCoroutine = StartCoroutine(EndMatch());
        }

        private IEnumerator EndMatch()
        {
            EventManager.SendEvent(Constants.MatchEnded);
            panelSwitcher.SwitchPressed(matchStatisticPanel);
            if (!PhotonNetwork.IsMasterClient)
            {
                _matchEndCoroutine = null;
                yield break;
            }
            yield return new WaitForSecondsRealtime((float) _matchStatisticsActiveTimeSpan.TotalSeconds);
            if (_currentRound < AmountOfRounds)
            {
                StartMatch();
            }
            else
            {
                photonView.RPC("ShowGameResults", RpcTarget.All);
            }
            _matchEndCoroutine = null;
        }

        [PunRPC]
        private void ShowGameResults()
        {
            panelSwitcher.SwitchPressed(gameResultPanel);
        }
        
        private void WaitForOtherPlayers(DateTime endDateTime, Action onTimerFinished)
        {
            waitingForOpponentsGameObject.SetActive(true);
            _timerCoroutine ??= StartCoroutine(StartTimer(endDateTime, onTimerFinished));
        }

        private IEnumerator StartTimer(DateTime endDateTime, Action onTimerFinished)
        {
            while (endDateTime > DateTime.Now)
            {
                Timer = endDateTime - DateTime.Now;
                yield return new WaitForFixedUpdate();
            }
            _timerCoroutine = null;
            onTimerFinished?.Invoke();
        }

        public static TeamEnum AddToTeam(PlayerController playerController)
        {
            if (PlayerControllersTeam1.Count == PlayerControllersTeam2.Count)
            {
                PlayerControllersTeam1.Add(playerController);
                return TeamEnum.Team1;
            }
            if (PlayerControllersTeam1.Count > PlayerControllersTeam2.Count)
            {
                PlayerControllersTeam2.Add(playerController);
                return TeamEnum.Team2;
            }
            if (PlayerControllersTeam1.Count < PlayerControllersTeam2.Count)
            {
                PlayerControllersTeam1.Add(playerController);
                return TeamEnum.Team1;
            }
            return default;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting && PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(Timer.ToString());
            }
            else if (stream.IsReading)
            {
                Timer = TimeSpan.Parse(stream.ReceiveNext().ToString());
            }
        }

        private static void UpdateRoomProperties()
        {
            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            hashtable[RoomProvider.CurrentRoundPropertyId] = _currentRound;
            RoomProvider.UpdateProperties(hashtable);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if(!PlayerManager.CurrentPlayerController.PhotonView.IsMine)
                return;
            if (PhotonNetwork.IsMasterClient)
                return;
            _currentRound = (int) propertiesThatChanged[RoomProvider.CurrentRoundPropertyId];
        }
    }
}