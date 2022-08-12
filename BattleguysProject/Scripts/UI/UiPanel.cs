using Tools;
using UnityEngine;

namespace UI
{
    public abstract class UiPanel : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] protected PanelType panelType;
        [SerializeField] protected GameObject container;
        [SerializeField] protected bool isDimmedScreenActive;
        [SerializeField] protected DimmedBackgroundPanel dimmedBackgroundPanel;
#pragma warning restore 649

        public PanelType PanelType => panelType;
        public bool IsPanelActive => container.activeInHierarchy;

        public virtual void Show()
        {
            container.SetActive(true);
            if (isDimmedScreenActive)
                dimmedBackgroundPanel.SetActiveDimmedScreen(isDimmedScreenActive, transform, Hide);
        }

        public virtual void Hide()
        {
            container.SetActive(false);
            if (isDimmedScreenActive)
                dimmedBackgroundPanel.SetActiveDimmedScreen(false);
        }

        public virtual void ShowComingSoonPopup()
        {
            EventManager.SendEvent(Constants.ComingSoonPopupId);
        }
    }
}