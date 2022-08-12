using System;
using MatchMaking;
using UI.HomeScene.HeroStats;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Player
{
    public class PlayerInfo : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerNickname playerNickname;
        [SerializeField] private SelectedHeroData selectedHeroData;
#pragma warning restore 649

        #region Constants
        private const string IsInitializedPropertyId = "IsInitializedId";
        private const string SpawnPropertyId = "SpawnId";
        private const string TeamPropertyId = "TeamId";
        private const string PlayerStatsPropertyId = "PlayerStatsId";
        private const string PlayerIsRevivedId = "PlayerIsRevivedId";
        #endregion
        
        private float _currentHealth;
        private string _nickname;

        public Hashtable PropertiesHashtable { get; private set; } = new Hashtable();
        public MatchProvider.TeamEnum TeamEnum { get; private set; } = MatchProvider.TeamEnum.None;
        public (int id, Transform transform) SpawnPoint { get; private set; } = (-1, null);
        public bool IsDead { get; private set; }
        public HeroStat[] HeroStats { get; private set; }
        public event Action OnHealthChanged;
        public event Action OnDeath;
        public event Action OnRevive;

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                OnHealthChanged?.Invoke();
                IsDead = _currentHealth <= 0;
                if (_currentHealth <= 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }
        
        public string Nickname
        {
            get => _nickname;
            private set
            {
                _nickname = value;
                playerNickname.SetNickname(_nickname);
                playerNickname.SetColor(TeamEnum);
            } 
        }

        private void Awake()
        {
            UpdateStats(selectedHeroData.HeroStats);
        }

        public void Init()
        {
            TeamEnum = MatchProvider.AddToTeam(playerController);
            SpawnPoint = SpawnPointProvider.GetSpawnPoint(TeamEnum);
            PropertiesHashtable[TeamPropertyId] = (int) TeamEnum;
            PropertiesHashtable[SpawnPropertyId] = SpawnPoint.id;
            PropertiesHashtable[PlayerIsRevivedId] = false;
            PropertiesHashtable[IsInitializedPropertyId] = true;
        }

        public void OnPropertiesUpdate(Hashtable newHashtable)
        {
            if (!newHashtable.ContainsKey(IsInitializedPropertyId))
                return;
            PropertiesHashtable = newHashtable;
            TeamEnum = (MatchProvider.TeamEnum) PropertiesHashtable[TeamPropertyId];
            int spawnPointId = (int) PropertiesHashtable[SpawnPropertyId];
            SpawnPoint = (spawnPointId, SpawnPointProvider.GetSpawnPoint(TeamEnum, spawnPointId));
            UpdateStats(SerializeUtility.SerializeUtility.DeserializeJSon<PlayerStatsWrapper>(
                (string) PropertiesHashtable[PlayerStatsPropertyId]).heroStats);
            Nickname = playerController.PhotonView.Controller.NickName;
            if ((bool) PropertiesHashtable[PlayerIsRevivedId])
                OnRevive?.Invoke();
        }

        public void ReceiveDamage(float damage)
        {
            CurrentHealth -= damage;
        }

        private void UpdateStats(HeroStat[] stats)
        {
            HeroStats = stats;
            CurrentHealth = HeroStats[(int) HeroStatType.Health].Value;
            PropertiesHashtable[PlayerStatsPropertyId] =
                SerializeUtility.SerializeUtility.SerializeToJSon(new PlayerStatsWrapper(stats));
        }

        public void Revive()
        {
            CurrentHealth = HeroStats[(int) HeroStatType.Health].Value;
            PropertiesHashtable[PlayerIsRevivedId] = true;
        }
    }
}