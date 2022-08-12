using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameModes.CaptureZone;
using Multiplayer;
using Photon.Pun;
using Player;

namespace MatchStatistic
{
    public class MatchStatistic : MonoBehaviourPunCallbacks
    {
#pragma warning disable 649
    
#pragma warning restore 649
        
        [Serializable]
        private class Statistics
        {
            public List<PlayerStatistic> playerStatistics;

            public Statistics(List<PlayerStatistic> playerStatistics)
            {
                this.playerStatistics = playerStatistics;
            }
        }
        
        private static readonly Dictionary<int, PlayerStatistic> PlayerStatisticsDictionary =
            new Dictionary<int, PlayerStatistic>();

        private new void OnEnable()
        {
            base.OnEnable();
            PlayerManager.OnPlayerInstantiatedEvent += AddPlayer;
            PlayerManager.OnPlayerDestroyedEvent += RemovePlayer;
            PlayerManager.OnPlayerDiedEvent += OnPlayerDied;
            CaptureZone.OnPlayerCaptureEvent += OnPlayerCapture;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            PlayerManager.OnPlayerInstantiatedEvent -= AddPlayer;
            PlayerManager.OnPlayerDestroyedEvent -= RemovePlayer;
            PlayerManager.OnPlayerDiedEvent -= OnPlayerDied;
            CaptureZone.OnPlayerCaptureEvent -= OnPlayerCapture;
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if(!PlayerManager.CurrentPlayerController.PhotonView.IsMine)
                return;
            if (PhotonNetwork.IsMasterClient)
                return;
            Statistics statistics =
                SerializeUtility.SerializeUtility.DeserializeJSon<Statistics>(
                    propertiesThatChanged[RoomProvider.MatchStatisticsPropertyId].ToString());
            if (statistics is null)
                return;
            PlayerStatisticsDictionary.Clear();
            foreach (var playerStatistic in statistics.playerStatistics)
            {
                PlayerStatisticsDictionary[playerStatistic.photonViewID] = playerStatistic;
            }
        }

        private static void AddPlayer(int photonViewId)
        {
            PlayerStatisticsDictionary[photonViewId] =
                new PlayerStatistic(photonViewId);
        }

        private static void RemovePlayer(int photonViewId)
        {
            if (PlayerStatisticsDictionary.ContainsKey(photonViewId))
                PlayerStatisticsDictionary.Remove(photonViewId);
        }

        private static void OnPlayerDied(int photonViewId, int photonViewIdKiller)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            PlayerStatisticsDictionary[photonViewId].AddDeath();
            PlayerStatisticsDictionary[photonViewIdKiller].AddKill();
            UpdateRoomProperties();
        }

        private static void OnPlayerCapture(int photonViewId)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            PlayerStatisticsDictionary[photonViewId].AddCapture();
            UpdateRoomProperties();
        }

        public static List<PlayerStatistic> GetPlayerStatistics()
        {
            return new List<PlayerStatistic>(PlayerStatisticsDictionary.Values);
        }

        private static void UpdateRoomProperties()
        {
            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            hashtable[RoomProvider.MatchStatisticsPropertyId] =
                SerializeUtility.SerializeUtility.SerializeToJSon(new Statistics(GetPlayerStatistics()));
            RoomProvider.UpdateProperties(hashtable);
        }
    }
}