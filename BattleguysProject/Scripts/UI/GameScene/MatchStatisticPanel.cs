using System.Collections.Generic;
using MatchMaking;
using MatchStatistic;
using Player;
using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class MatchStatisticPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI roundTextMeshProUGUI;
        [SerializeField] private List<PlayerStatisticUi> blueTeamPlayerStatisticUiList;
        [SerializeField] private List<PlayerStatisticUi> redTeamPlayerStatisticUiList;
#pragma warning restore 649

        private bool _isStatisticsActive = false;
        
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_isStatisticsActive)
                {
                    Hide();
                    _isStatisticsActive = false;
                }
                else
                {
                    Show();
                    _isStatisticsActive = true;
                }
            }
        }
#endif

        public override void Show()
        {
            Init();
            FillData();
            base.Show();
        }

        private void Init()
        {
            roundTextMeshProUGUI.text = "Round " + MatchProvider.CurrentRound;
            for (int i = 0; i < blueTeamPlayerStatisticUiList.Count; i++)
            {
                blueTeamPlayerStatisticUiList[i].gameObject.SetActive(false);
                redTeamPlayerStatisticUiList[i].gameObject.SetActive(false);
            }
        }

        private void FillData()
        {
            List<PlayerStatistic> playerStatisticsList = MatchStatistic.MatchStatistic.GetPlayerStatistics();
            (int bluePlayerId, int redPlayerId) teamStatisticId = (0, 0);
            foreach (var playerStatistic in playerStatisticsList)
            {
                if (PlayerManager.PlayerControllersDictionary[playerStatistic.photonViewID].TeamEnum == MatchProvider.TeamEnum.Team1)
                {
                    blueTeamPlayerStatisticUiList[teamStatisticId.bluePlayerId].gameObject.SetActive(true);
                    blueTeamPlayerStatisticUiList[teamStatisticId.bluePlayerId++].Set(playerStatistic);
                }
                if (PlayerManager.PlayerControllersDictionary[playerStatistic.photonViewID].TeamEnum == MatchProvider.TeamEnum.Team2)
                {
                    redTeamPlayerStatisticUiList[teamStatisticId.redPlayerId].gameObject.SetActive(true);
                    redTeamPlayerStatisticUiList[teamStatisticId.redPlayerId++].Set(playerStatistic);
                }
            }
        }
    }
}