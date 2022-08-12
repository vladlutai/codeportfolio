using System;
using Tools;
using UnityEngine;

namespace UI.HomeScene
{
    public class ComingSoonPopup : UiPanel
    {
#pragma warning disable 649
    
#pragma warning restore 649

        private void OnEnable()
        {
            EventManager.StartListening(Constants.ComingSoonPopupId, Show);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Constants.ComingSoonPopupId, Show);
        }
    }
}