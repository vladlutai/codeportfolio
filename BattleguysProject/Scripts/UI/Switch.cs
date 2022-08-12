using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class Switch : MonoBehaviour, IPointerClickHandler
    {
#pragma warning disable 649
        [SerializeField] private Transform handleTransform;
        [SerializeField] private Image handleOnImage;
        [SerializeField] private TextMeshProUGUI switchOffTextMeshPro;
        [SerializeField] private TextMeshProUGUI switchOnTextMeshPro;
        [SerializeField] private Image backgroundOnImage;
#pragma warning restore 649

        private bool _isOn = false;

        #region Constants
        private const float SwitchingTime = 0.15f;
        private readonly (float offX, float onX) _switchMoveRangeX = (-50f, 50f);
        #endregion

        public void OnPointerClick(PointerEventData eventData)
        {
            _isOn = !_isOn;
            SetSwitch();
        }

        private void SetSwitch()
        {
            backgroundOnImage.DOKill();
            backgroundOnImage.DOFade(_isOn ? 1f : 0f, SwitchingTime).SetEase(Ease.OutCubic);

            handleOnImage.DOKill();
            handleOnImage.DOFade(_isOn ? 1f : 0f, SwitchingTime).SetEase(Ease.OutCubic);

            handleTransform.DOKill();
            handleTransform.DOLocalMoveX(_isOn ? _switchMoveRangeX.onX : _switchMoveRangeX.offX, SwitchingTime)
                .SetEase(Ease.InOutCubic);

            switchOffTextMeshPro.DOFade(_isOn ? 0f : 1f, SwitchingTime).SetEase(Ease.OutCubic);
            switchOnTextMeshPro.DOFade(_isOn ? 1f : 0f, SwitchingTime).SetEase(Ease.OutCubic);
        }
    }
}