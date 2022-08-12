using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "PlayerSkinData", fileName = "PlayerSkinData")]
    public class PlayerSkinData : ScriptableObject
    {
#pragma warning disable 649
        [SerializeField] private List<PlayerSkin> playerSkinsList;
#pragma warning restore 649

        public List<PlayerSkin> PlayerSkinsList => playerSkinsList;
    }
}