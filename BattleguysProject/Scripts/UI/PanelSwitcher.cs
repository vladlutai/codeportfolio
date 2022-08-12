using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum PanelType
    {
        None = 0,
        Home,
        Shop,
        Settings,
        Equipment,
        Heroes,
        DailyReward,
        ReceivedItems,
        GameMode,
        ArenaType,
        Lobby,
        Loading,
        MatchStatistics,
        Died,
        MatchResult,
        Game
    }
    
    public class PanelSwitcher : MonoBehaviour
    {

        [Serializable]
        public class Panel
        {
#pragma warning disable 649
            [SerializeField] private UiPanel uiPanel;
            [SerializeField] private Sprite backgroundSprite;
#pragma warning restore 649

            public UiPanel UIPanel => uiPanel;
            public Sprite BackgroundSprite => backgroundSprite;

            public void SetActive(bool value)
            {
                switch (value)
                {
                    case true:
                        uiPanel.Show();
                        break;
                    
                    case false:
                        uiPanel.Hide();
                        break;
                }
            }
        }
        
#pragma warning disable 649
        [SerializeField] private PanelType startPanel;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private List<Panel> panelsList;
#pragma warning restore 649

        private PanelType _currentPanel = PanelType.None;
        private PanelType _previousPanel = PanelType.None;
        private readonly Dictionary<PanelType, Panel> _panelDictionary = new Dictionary<PanelType, Panel>();

        private PanelType CurrentPanel
        {
            get => _currentPanel;
            set
            {
                _previousPanel = _currentPanel;
                _currentPanel = value;
                SetBackground();
                Switch();
            }
        }
        
        private void Switch()
        {
            if (_previousPanel != PanelType.None)
                _panelDictionary[_previousPanel].SetActive(false);
            if (_currentPanel != PanelType.None)
                _panelDictionary[_currentPanel].SetActive(true);
        }

        private void SetBackground()
        {
            Sprite sprite = _panelDictionary[_currentPanel].BackgroundSprite;
            backgroundImage.enabled = sprite != null;
            if (sprite == null)
            {
                return;
            }
            backgroundImage.sprite = _panelDictionary[_currentPanel].BackgroundSprite;
        }

        private void Awake()
        {
            Init();
            CurrentPanel = startPanel;
        }

        private void Init()
        {
            foreach (var panel in panelsList)
            {
                _panelDictionary[panel.UIPanel.PanelType] = panel;
            }
        }

        public void SwitchPressed(UiPanel uiPanel)
        {
            CurrentPanel = uiPanel.PanelType;
        }
    }
}