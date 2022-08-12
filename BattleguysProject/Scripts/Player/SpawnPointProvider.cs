using System.Collections.Generic;
using MatchMaking;
using UnityEngine;

namespace Player
{
    public class SpawnPointProvider : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private List<SpawnPoint> spawnPointsTeam1List;
        [SerializeField] private List<SpawnPoint> spawnPointsTeam2List;
#pragma warning restore 649
        
        private static List<SpawnPoint> _spawnPointsTeam1List;
        private static List<SpawnPoint> _spawnPointsTeam2List;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _spawnPointsTeam1List = new List<SpawnPoint>(spawnPointsTeam1List);
            _spawnPointsTeam2List = new List<SpawnPoint>(spawnPointsTeam2List);
            spawnPointsTeam1List = null;
            spawnPointsTeam2List = null;
        }

        public static (int id, Transform transform) GetSpawnPoint(MatchProvider.TeamEnum teamEnum)
        {
            List<SpawnPoint> spawnPointsList =
                teamEnum == MatchProvider.TeamEnum.Team1 ? _spawnPointsTeam1List : _spawnPointsTeam2List;
            for (int i = 0; i < spawnPointsList.Count; i++)
            {
                if (spawnPointsList[i].State == SpawnPoint.StateEnum.UnOccupied)
                {
                    spawnPointsList[i].UpdateState(SpawnPoint.StateEnum.Occupied);
                    return (i, spawnPointsList[i].transform);
                }
            }
            return default;
        }

        public static Transform GetSpawnPoint(MatchProvider.TeamEnum teamEnum, int id)
        {
            List<SpawnPoint> spawnPointsList =
                teamEnum == MatchProvider.TeamEnum.Team1 ? _spawnPointsTeam1List : _spawnPointsTeam2List;
            return spawnPointsList[id].transform;
        }
    }
}