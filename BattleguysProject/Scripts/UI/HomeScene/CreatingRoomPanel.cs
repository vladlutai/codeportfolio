using Multiplayer;
using TMPro;
using UnityEngine;

namespace UI.HomeScene
{
    public class CreatingRoomPanel : UiPanel
    {
        
#pragma warning disable 649
        [SerializeField] private TMP_InputField roomNameInputField;
        [SerializeField] private TextMeshProUGUI roomTagTextMeshPro;
        [SerializeField] private TextMeshProUGUI roomAccessTextMeshPro;
        [SerializeField] private TMP_InputField roomPasswordInputField;
        [SerializeField] private TextMeshProUGUI playersCountTextMeshPro;
        [SerializeField] private TextMeshProUGUI roundCountTextMeshPro;
        [SerializeField] private TextMeshProUGUI mapNameTextMeshPro;
        [SerializeField] private TextMeshProUGUI victoryConditionTextMeshPro;
#pragma warning restore 649

        private string _name;
        private string _tag;
        private RoomAccess _access;
        private string _password;
        private byte _maxPlayers;
        private int _roundCount;
        private string _mapName;
        private string _victoryCondition;
        
        public void CreateRoomPressed()
        {
            CheckInput();
            RoomProvider.CreateRoom(new RoomInfo(_name, _tag, 0, _maxPlayers, _password, _roundCount, _mapName,
                _victoryCondition, _access));
        }

        private void CheckInput()
        {
            _name = roomNameInputField.text;
            _tag = roomTagTextMeshPro.text;
            _access = (RoomAccess) System.Enum.Parse(typeof(RoomAccess), roomAccessTextMeshPro.text);
            _password = roomPasswordInputField.text;
            _maxPlayers = byte.Parse(playersCountTextMeshPro.text);
            _roundCount = int.Parse(roundCountTextMeshPro.text);
            _mapName = mapNameTextMeshPro.text;
            _victoryCondition = victoryConditionTextMeshPro.text;
        }
    }
}