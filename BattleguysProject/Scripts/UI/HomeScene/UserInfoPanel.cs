using TMPro;
using UnityEngine;
using User;

namespace UI.HomeScene
{
    public class UserInfoPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI nicknameTextMeshPro;
        [SerializeField] private NicknamePopup nicknamePopup;
#pragma warning restore 649


        private void OnEnable()
        {
            nicknamePopup.OnNicknameChanged += OnNicknameChangedHandler;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            SetNickname(UserInfo.Nickname);
        }

        private void OnDisable()
        {
            nicknamePopup.OnNicknameChanged -= OnNicknameChangedHandler;
        }

        private void SetNickname(string newNickname)
        {
            nicknameTextMeshPro.text = newNickname;
        }

        private void OnNicknameChangedHandler(string newNickname)
        {
            UserInfo.ChangeNickname(newNickname);
            SetNickname(newNickname);
        }
    }
}