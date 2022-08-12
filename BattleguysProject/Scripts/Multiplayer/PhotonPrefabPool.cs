using Photon.Pun;
using UnityEngine;

namespace Multiplayer
{
    public class PhotonPrefabPool : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject playerPrefab;
#pragma warning restore 649

        public string PlayerPrefabName => playerPrefab.name;

        public void Awake()
        {
            if (PhotonNetwork.PrefabPool is DefaultPool defaultPool &&
                !defaultPool.ResourceCache.ContainsKey(playerPrefab.name))
            {
                defaultPool.ResourceCache.Add(playerPrefab.name, playerPrefab);
            }
        }
    }
}