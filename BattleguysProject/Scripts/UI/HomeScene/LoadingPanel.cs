using UI.LoadingScene;
using UnityEngine;

namespace UI.HomeScene
{
    public class LoadingPanel : UiPanel
    {
#pragma warning disable 649
        [SerializeField] private LoadingSlider loadingSlider;
#pragma warning restore 649

        public override void Show()
        {
            base.Show();
            loadingSlider.SetValue(0f);
            loadingSlider.SetActive(true);
        }

        public void InitLoadingSlider(int maxValue)
        {
            loadingSlider.Init(maxValue);
        }

        public void UpdateLoadingSliderValue(float value)
        {
            loadingSlider.SetValue(value, $"<color=#00FFFF>Loading...</color>{value:F0}%");
        }
    }
}