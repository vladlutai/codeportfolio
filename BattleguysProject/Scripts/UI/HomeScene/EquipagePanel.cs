using System;

namespace UI.HomeScene
{
    public class EquipagePanel : UiPanel
    {
        public event Action OnHide;

        public override void Hide()
        {
            base.Hide();
            OnHide?.Invoke();
        }
    }
}