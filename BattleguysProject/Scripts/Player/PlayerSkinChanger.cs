using UnityEngine;

namespace Player
{
    public class PlayerSkinChanger : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Transform skinParentGameObject;
        [SerializeField] private PlayerSkinData playerSkinData;
        [SerializeField] private PlayerController playerController;
#pragma warning restore 649

        private int _currentSkinId = -1;

        public void ChangeSkin()
        {
            GameObject currentSkin = skinParentGameObject.transform.GetChild(0).gameObject;
            Destroy(currentSkin);
            if (_currentSkinId < playerSkinData.PlayerSkinsList.Count - 1)
            {
                _currentSkinId++;
            }
            else
            {
                _currentSkinId = 0;
            }
            PlayerSkin skin = Instantiate(playerSkinData.PlayerSkinsList[_currentSkinId], skinParentGameObject, true);
            skin.transform.localPosition = Vector3.zero;
            skin.transform.localRotation = Quaternion.identity;
            playerAnimator.avatar = skin.Avatar;
        }
    }
}