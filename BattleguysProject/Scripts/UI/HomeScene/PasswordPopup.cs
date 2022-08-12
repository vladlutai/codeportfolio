using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.HomeScene
{
    public class PasswordPopup : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private CanvasGroup errorMessageCanvasGroup;
        [SerializeField] private TMP_InputField passwordInputField;
#pragma warning restore 649

        public Action<string> CheckPasswordAction;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            passwordInputField.text = string.Empty;
            errorMessageCanvasGroup.DOFade(0f, 0f);
        }
        
        public override void Show()
        {
            base.Show();
            Init();
        }

        public void OkButtonPressed()
        {
            CheckPasswordAction?.Invoke(passwordInputField.text);
        }

        public void ShowErrorMessage()
        {
            errorMessageCanvasGroup.DOKill();
            errorMessageCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutCubic);
        }
    }
}