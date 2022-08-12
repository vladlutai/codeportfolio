using UnityEngine;

namespace Player
{
    public class PlayerCountry : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private SpriteRenderer countrySpriteRenderer;
#pragma warning restore 649

        public void SetCountry(Sprite sprite)
        {
            countrySpriteRenderer.sprite = sprite;
        }
    }
}