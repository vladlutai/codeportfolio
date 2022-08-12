using MatchMaking;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerNickname : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TextMeshPro nicknameTextMeshPro;
#pragma warning restore 649

        public void SetNickname(string nickname)
        {
            nicknameTextMeshPro.text = nickname;
        }

        public void SetColor(MatchProvider.TeamEnum team)
        {
            Color color = Color.white;
            switch (team)
            {
                case MatchProvider.TeamEnum.Team1:
                    color = Color.blue;
                    break;
                
                case MatchProvider.TeamEnum.Team2:
                    color = Color.red;
                    break;
            }
            nicknameTextMeshPro.color = color;
        }
    }
}