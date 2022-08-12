using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    public class EventManager : MonoBehaviour
    {
        [Serializable]
        public class GameEvent : UnityEvent
        {
        }

        private static EventManager _eventManager;
        private Dictionary<string, GameEvent> _eventDictionary;

        private static EventManager Instance
        {
            get
            {
                if (_eventManager == null)
                {
                    _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                    if (_eventManager == null)
                    {
                        Debug.LogError("There needs to be one active EventManager");
                    }
                    else
                    {
                        _eventManager.Init();
                    }
                }
                return _eventManager;
            }
        }

        private void Init()
        {
            if(_eventDictionary == null)
                _eventDictionary = new Dictionary<string, GameEvent>();
        }

        public static void StartListening(string eventName, UnityAction listener)
        {
            GameEvent gameEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out gameEvent))
            {
                gameEvent.AddListener(listener);
            }
            else
            {
                gameEvent = new GameEvent();
                gameEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, gameEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            if(_eventManager == null)
                return;
            GameEvent gameEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out gameEvent))
            {
                gameEvent.RemoveListener(listener);
            }
        }

        public static void SendEvent(string eventName)
        {
            GameEvent gameEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out gameEvent))
            {
                gameEvent.Invoke();
            }
        }
    }
}