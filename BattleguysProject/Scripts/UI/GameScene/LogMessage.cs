using System;
using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace UI.GameScene
{
    public class LogMessage : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI logMessageTextMeshPro;
#pragma warning restore 649

        #region Constants
        private const float FadeDuration = 1f;
        private const float DefaultFadeTimer = 5f;
        private const float DecreaseFadeTimerCoef = 0.7f;
        private const float DefaultCanvasGroupAlpha = 1f;
        #endregion

        private float _timer;
        private Action _onDisable;

        public void Init(Action onDisable)
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = DefaultCanvasGroupAlpha;
            _onDisable = onDisable;
        }

        public void Set(string message, float fadeTimer = DefaultFadeTimer)
        {
            if (!gameObject.activeInHierarchy)
                return;
            logMessageTextMeshPro.text = message;
            _timer = fadeTimer;
            StartCoroutine(Timer());
        }

        public void DecreaseFadeTimer()
        {
            _timer *= DecreaseFadeTimerCoef;
        }

        private IEnumerator Timer()
        {
            float elapsed = 0;
            while ((elapsed += Time.deltaTime) < _timer)
            {
                yield return null;
            }
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, FadeDuration).SetEase(Ease.Flash).OnComplete(() => _onDisable?.Invoke());
        }
    }
}