using System;
using TMPro;
using UnityEngine;

namespace UI.LoadingScene
{
    public class DownloadingStatistics : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI remainingTimeTextMeshPro;
        [SerializeField] private TextMeshProUGUI downloadingSpeedTextMeshPro;
#pragma warning restore 649

        #region Constants
        private const string TimerTextFormat = @"m\:ss";
        #endregion

        public void Set(TimeSpan remainingTime, string downloadingSpeed)
        {
            remainingTimeTextMeshPro.text = remainingTime.ToString(TimerTextFormat);
            downloadingSpeedTextMeshPro.text = downloadingSpeed;
        }
    }
}