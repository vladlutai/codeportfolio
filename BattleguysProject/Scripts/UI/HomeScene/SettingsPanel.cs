using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HomeScene
{
    public class SettingsPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private LanguagePopup languagePopup;
        [SerializeField] private Image languageImage;
        [SerializeField] private TextMeshProUGUI languageNameTextMeshPro;
        [SerializeField] private List<Sprite> countryFlags;
        [SerializeField] private List<string> countryNames;
#pragma warning restore 649

        #region Constants
        private const string GooglePlayGameLink = "https://apple.com";
        private const string AppStoreGameLink = "https://play.google.com";
        private const string PrivacyPolicyLink = "https://www.airlab.ai/privacy-policy/";
        private const string TermsOfUseLink = "https://airlab.ai/terms-of-service/";
        private const string GooglePlayOtherGamesLink = "https://play.google.com/store/apps/dev?id=6266588510479054902";
        private const string AppStoreOtherGamesLink = "https://apps.apple.com/be/developer/airlab-ltd/id1521935880";
        #endregion

        private void OnEnable()
        {
            languagePopup.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnDisable()
        {
            languagePopup.OnLanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(int id)
        {
            languageImage.sprite = countryFlags[id];
            languageNameTextMeshPro.text = countryNames[id];
        }

        public void RatePressed()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    Application.OpenURL(GooglePlayGameLink);
                    return;
                
                case RuntimePlatform.IPhonePlayer:
                    Application.OpenURL(AppStoreGameLink);
                    return;
            }
        }

        public void PrivacyPolicyPressed()
        {
            Application.OpenURL(PrivacyPolicyLink);
        }

        public void TermsOfUsePressed()
        {
            Application.OpenURL(TermsOfUseLink);
        }

        public void OtherGamesPressed()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    Application.OpenURL(GooglePlayOtherGamesLink);
                    return;
                
                case RuntimePlatform.IPhonePlayer:
                    Application.OpenURL(AppStoreOtherGamesLink);
                    return;
            }
            
            Application.OpenURL(GooglePlayOtherGamesLink);
            Application.OpenURL(AppStoreOtherGamesLink);
        }
    }
}