using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using Unity.DeviceSimulator;
#endif

namespace Tools
{
    [RequireComponent(typeof(RectTransform)), ExecuteAlways]
    public class SafeArea : MonoBehaviour
    {
#pragma warning disable 649

#pragma warning restore 649

        #region MyRegion
        private const string SafeAreaTag = "SafeArea";
        private const int EditorRecalculateSafeAreaDelay = 100;
        #endregion

        private static bool _isOnDeviceChangeSubscribed;

        private void Awake()
        {
            ReCalculateSafeArea();
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (_isOnDeviceChangeSubscribed) return;
            DeviceSimulatorCallbacks.OnDeviceChange += () =>
            {
                Recalculate();
                async void Recalculate()
                {
                    await Task.Run(() => Thread.Sleep(EditorRecalculateSafeAreaDelay));
                    ReCalculateSafeArea();
                }
            };
            _isOnDeviceChangeSubscribed = true;
        }
#endif

        #if UNITY_EDITOR
        [MenuItem("Custom/Init Safe Area", priority = 2)]
        #endif
        private static void ReCalculateSafeArea()
        {
            RectTransform rectTransform = GameObject.FindWithTag(SafeAreaTag).GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.Log("RectTransform at SafeArea is null");
                return;
            }
            (Vector2 anchorMin, Vector2 anchorMax) safeArea = CalculateSafeArea();
            rectTransform.anchorMin = safeArea.anchorMin;
            rectTransform.anchorMax = safeArea.anchorMax;
        }

        private static (Vector2 anchorMin, Vector2 anchorMax) CalculateSafeArea()
        {
            Rect safeAreaRect = Screen.safeArea;
            Vector2 anchorMin = safeAreaRect.position;
            Vector2 anchorMax = anchorMin + safeAreaRect.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            return (anchorMin, anchorMax);
        }
    }
}