using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonPro : Button, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private enum ButtonAnimationType
        {
            None,
            Elastic,
            Back
        }
        
#pragma warning disable 649
        
#pragma warning restore 649

        private bool _isDown;
        private Transform _transform;
        private ButtonAnimationType _buttonAnimationType = ButtonAnimationType.Elastic;

        private new void Awake()
        {
            _transform = transform;
        }

        public new void OnPointerDown(PointerEventData eventData)
        {
            _isDown = true;
            _transform.DOKill();
            _transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutCubic).SetUpdate(true);
        }

        public new void OnPointerUp(PointerEventData eventData)
        {
            if(!_isDown)
                return;
            _isDown = false;
            PlayAnimation();
        }

        public new void OnPointerExit(PointerEventData eventData)
        {
            if(!_isDown)
                return;
            _isDown = false;
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            transform.DOKill();
            switch (_buttonAnimationType)
            {
                case ButtonAnimationType.Elastic:
                    transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic).SetUpdate(true);
                    break;

                case ButtonAnimationType.Back:
                    transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
                    break;

                case ButtonAnimationType.None:
                    transform.DOScale(1f, 0.25f).SetEase(Ease.OutCubic).SetUpdate(true);
                    break;
            }
        }
    }
}