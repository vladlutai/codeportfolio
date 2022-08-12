using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DimmedBackgroundPanel : MonoBehaviour, IPointerClickHandler
    {
#pragma warning disable 649
        [SerializeField] private GameObject dimmedScreenGameObject;
#pragma warning restore 649

        private Action _onClick;

        public void SetActiveDimmedScreen(bool value, Transform parentTransform = null, Action onClick = null)
        {
            _onClick = onClick;
            if (parentTransform != null)
                dimmedScreenGameObject.transform.SetParent(parentTransform);
            dimmedScreenGameObject.transform.SetAsFirstSibling();
            dimmedScreenGameObject.SetActive(value);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke();
            _onClick = null;
        }
    }
}