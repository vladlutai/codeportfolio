using Tools;

namespace Multiplayer
{
    public enum RoomAccess
    {
        Public,
        Private
    }
    
    public class RoomInfo
    {
        public string Name { get; }
        public string Tag { get; }
        public int PlayerCount { get; }
        public byte MaxPlayers { get; }
        public string Password { get; }
        public int RoundCount { get; }
        public string MapName { get; }
        public string VictoryCondition { get; }
        public RoomAccess RoomAccess { get; }
            
        public RoomInfo(Photon.Realtime.RoomInfo roomInfo)
        {
            Name = roomInfo.Name;
            Tag = (string) roomInfo.CustomProperties[RoomProvider.TagCustomProperty];
            PlayerCount = roomInfo.PlayerCount;
            MaxPlayers = roomInfo.MaxPlayers;
            Password = (string) roomInfo.CustomProperties[RoomProvider.PasswordCustomProperty];
            MapName = (string) roomInfo.CustomProperties[RoomProvider.MapCustomProperty];
            VictoryCondition = (string) roomInfo.CustomProperties[RoomProvider.VictoryConditionCustomProperty];
            RoomAccess = (RoomAccess) System.Enum.Parse(typeof(RoomAccess),
                roomInfo.CustomProperties[RoomProvider.AccessCustomProperty].ToString());
        }

        public RoomInfo(string name, string tag = "#Test", int playerCount = 0, byte maxPlayers = 6,
            string password = "", int roundCount = 3, string mapName = "Map", string victoryCondition = "Capture",
            RoomAccess roomAccess = RoomAccess.Public)
        {
            Name = name;
            Tag = tag;
            PlayerCount = playerCount;
            MaxPlayers = maxPlayers;
            Password = HashingUtility.GetHash(password);
            RoundCount = roundCount;
            MapName = mapName;
            VictoryCondition = victoryCondition;
            RoomAccess = roomAccess;
        }
    }
}