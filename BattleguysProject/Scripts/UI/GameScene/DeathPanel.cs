using System;
using System.Collections;
using Multiplayer;
using UnityEngine;
using TMPro;
using Player;

namespace UI.GameScene
{
    public class DeathPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private TMP_Text timerText;
#pragma warning restore 649

        #region Constants
        private const string TimerTextFormat = @"m\:ss";
        private readonly TimeSpan _playerReviveTimeTimeSpan = new TimeSpan(0, 0, 0, 15);
        #endregion

        private TimeSpan _timer;
        private Coroutine _timerCoroutine;

        private TimeSpan Timer
        {
            get => _timer;
            set
            {
                _timer = value;
                timerText.text = _timer.ToString(TimerTextFormat);
            }
        }

        private void Start()
        {
            PlayerManager.OnCurrentPlayerDiedEvent += Init;
        }

        public void Init()
        {
            Show();
            DeathTimer(ServerTimeProvider.ServerTime.Add(_playerReviveTimeTimeSpan), Revive);
        }

        public void ReviveForGems()
        {
            Revive();
        }

        public void ReviveForAds()
        {
            Revive();
        }

        private void Revive()
        {
            Hide();
            StopDeathTimer();
            PlayerManager.CurrentPlayerController.Revive();
        }

        private void DeathTimer(DateTime endDateTime, Action onTimerFinished)
        {
            _timerCoroutine ??= StartCoroutine(StartTimer(endDateTime, onTimerFinished));
        }

        private void StopDeathTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }

        private IEnumerator StartTimer(DateTime endDateTime, Action onTimerFinished)
        {
            while (endDateTime > ServerTimeProvider.ServerTime)
            {
                Timer = endDateTime - ServerTimeProvider.ServerTime;
                yield return new WaitForFixedUpdate();
            }
            _timerCoroutine = null;
            onTimerFinished?.Invoke();
        }       
    }
}