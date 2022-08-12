using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HomeScene
{
    public class LanguagePopup : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private RectTransform checkmark;
#pragma warning restore 649

        #region Constants
        private readonly Vector2 _checkmarkAnchorMin = new Vector2(1, 0);
        private readonly Vector2 _checkmarkAnchorMax = new Vector2(1, 0);
        private readonly Vector2 _checkmarkAnchoredPosition = new Vector2(-30, 40);
        #endregion

        public event Action<int> OnLanguageChanged;

        public void SelectLanguagePressed(int id)
        {
            OnLanguageChanged?.Invoke(id);
            checkmark.transform.SetParent(EventSystem.current.currentSelectedGameObject.transform);
            checkmark.anchorMin = _checkmarkAnchorMin;
            checkmark.anchorMax = _checkmarkAnchorMax;
            checkmark.anchoredPosition = _checkmarkAnchoredPosition;
        }
    }
}