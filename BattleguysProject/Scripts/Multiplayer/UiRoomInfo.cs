using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class UiRoomInfo : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI roomNameTextMeshPro;
        [SerializeField] private TextMeshProUGUI roomTagTextMeshPro;
        [SerializeField] private TextMeshProUGUI playerCountTextMeshPro;
        [SerializeField] private Slider playerCountSlider;
        [SerializeField] private TextMeshProUGUI victoryConditionTextMeshPro;
        [SerializeField] private TextMeshProUGUI accessTextMeshPro;
        [SerializeField] private Button joinButton;
#pragma warning restore 649

        public void Set(string name, string tag, int currentPlayersCount, int maxPlayerCount, string victoryCondition,
            RoomAccess access, Action listener)
        {
            roomNameTextMeshPro.text = name;
            roomTagTextMeshPro.text = tag;
            playerCountTextMeshPro.text = $"{currentPlayersCount}/{maxPlayerCount}";
            playerCountSlider.value = currentPlayersCount;
            playerCountSlider.maxValue = maxPlayerCount;
            victoryConditionTextMeshPro.text = victoryCondition;
            accessTextMeshPro.text = access.ToString();
            joinButton.onClick.AddListener(() => listener?.Invoke());
        }
    }
}