using Player;
using UI.HomeScene.HeroStats;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene
{
    public class PlayerInfoPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private Slider healthSlider;
#pragma warning restore 649

        private void Awake()
        {
            PlayerManager.OnCurrentPlayerSpawnedEvent += OnPlayerSpawnedEvent;
        }

        private void OnPlayerSpawnedEvent()
        {
            PlayerController player = PlayerManager.CurrentPlayerController;
            healthSlider.maxValue = player.PlayerInfo.HeroStats[(int) HeroStatType.Health].Value;
            healthSlider.value = player.PlayerInfo.CurrentHealth;
            player.PlayerInfo.OnHealthChanged += HealthChangedHandler;
        }

        private void HealthChangedHandler()
        {
            healthSlider.value = PlayerManager.CurrentPlayerController.PlayerInfo.CurrentHealth;
        }
    }
}