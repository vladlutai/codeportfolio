using TMPro;
using UnityEngine;

namespace Tools
{
    public class FpsDisplay : MonoBehaviour
    {
#pragma warning disable
        [Space(10)] [SerializeField] private TextMeshProUGUI fpsText;
        [SerializeField] [Range(0, 2f)] private float refreshRate = 1f;
#pragma warning restore

        private float _timer;
        private int _currentFps;
        private bool _isEnable = true;

        #region Constants
        private const int TargetFrameRate = 60;
        #endregion

        private void Starts()
        {
#if UNITY_EDITOR
            _isEnable = true;
#else
        _isEnable = false;
#endif
            Application.targetFrameRate = TargetFrameRate;
            fpsText.gameObject.SetActive(_isEnable);
        }

        private void Update()
        {
            if (!_isEnable)
                return;
            if (Time.unscaledTime > _timer)
            {
                _currentFps = (int) (1f / Time.unscaledDeltaTime);
                fpsText.text = _currentFps.ToString();
                _timer = Time.unscaledTime + refreshRate;
            }
        }
    }
}