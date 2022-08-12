using System;
using TMPro;
using UnityEngine;

namespace UI.HomeScene
{
    public class NicknamePopup : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private TMP_InputField nicknameTMPInputField;
#pragma warning restore 649

        public event Action<string> OnNicknameChanged;

        public void ChangeNicknamePressed()
        {
            string newNickname = nicknameTMPInputField.text;
            if (!string.IsNullOrEmpty(newNickname))
            {
                OnNicknameChanged?.Invoke(newNickname);
                Hide();
            }
        }
    }
}