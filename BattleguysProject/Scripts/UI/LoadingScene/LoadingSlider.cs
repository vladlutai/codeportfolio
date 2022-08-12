using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LoadingScene
{
    public class LoadingSlider : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject container;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI statusTextMeshPro;
#pragma warning restore 649

        private void Awake()
        {
            SetActive(false);
        }

        public void Init(int maxValue)
        {
            slider.maxValue = maxValue;
        }

        public void SetValue(float value, string progress = "<color=#00FFFF>N/A</color>")
        {
            slider.value = value;
            statusTextMeshPro.text = progress;
        }

        public void SetActive(bool value)
        {
            SetValue(0);
            container.SetActive(value);
        }
    }
}