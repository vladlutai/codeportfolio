using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.HomeScene
{
    public class SwitchContainer : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI containerTextMeshPro;
        [SerializeField] private List<string> contentList;
        [SerializeField] private UnityEvent<string> onValueChanged;
#pragma warning restore 649

        private int _currentId = 0;

        private void OnEnable()
        {
            Switch();
        }

        private void Switch()
        {
            if (_currentId < 0)
                _currentId = contentList.Count - 1;
            else if (_currentId >= contentList.Count)
            {
                _currentId = 0;
            }
            containerTextMeshPro.text = contentList[_currentId];
            onValueChanged?.Invoke(contentList[_currentId]);
        }
        
        public void LeftSwitchPressed()
        {
            _currentId--;
            Switch();
        }

        public void RightSwitchPressed()
        {
            _currentId++;
            Switch();
        }
    }
}