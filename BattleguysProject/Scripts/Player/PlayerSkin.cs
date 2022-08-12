using UnityEngine;

namespace Player
{
    public class PlayerSkin : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Avatar avatar;
        [SerializeField] private CharacterData characterData;
#pragma warning restore 649

        public Avatar Avatar => avatar;
        public CharacterData CharacterData => characterData;
    }
}