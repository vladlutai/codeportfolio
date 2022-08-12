using MatchMaking;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene
{
    public class PlayerStatisticUi : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image avatarImage;
        [SerializeField] private TextMeshProUGUI nicknameTextMeshPro;
        [SerializeField] private TextMeshProUGUI killAmountTextMeshPro;
        [SerializeField] private TextMeshProUGUI deathAmountTextMeshPro;
        [SerializeField] private TextMeshProUGUI captureAmountTextMeshPro;
        [SerializeField] private GameObject mvpGameObject;
        [SerializeField] private Sprite blueTeamBackgroundSprite;
        [SerializeField] private Sprite redTeamBackgroundSprite;
#pragma warning restore 649

        public void Set(MatchStatistic.PlayerStatistic playerStatistic)
        {
            PlayerInfo playerInfo = PlayerManager.PlayerControllersDictionary[playerStatistic.photonViewID].PlayerInfo;
            backgroundImage.sprite = playerInfo.TeamEnum == MatchProvider.TeamEnum.Team1
                ? blueTeamBackgroundSprite
                : redTeamBackgroundSprite;
            nicknameTextMeshPro.text = playerInfo.Nickname;
            killAmountTextMeshPro.text = playerStatistic.killCount.ToString();
            deathAmountTextMeshPro.text = playerStatistic.deathCount.ToString();
            captureAmountTextMeshPro.text = playerStatistic.captureCount.ToString();
        }
    }
}