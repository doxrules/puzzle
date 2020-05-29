namespace Events
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;
    using Events;

    public class EventManager
    {
        static EventManager _instance;
        private Dictionary<string, BaseEvent> _eventDictionary;
    
        public static EventManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EventManager {_eventDictionary = new Dictionary<string, BaseEvent>()};
                }

                return _instance;
            }
        }

        public void RegisterEvent(BaseEvent blastyEvent)
        {
            _eventDictionary.Add(blastyEvent.GetEventName(), blastyEvent);
        }

        public void UnRegisterEvent(BaseEvent blastyEvent)
        {
            if(_eventDictionary.ContainsKey(blastyEvent.GetEventName()))
            {
                _eventDictionary.Remove(blastyEvent.GetEventName());
            }
        }
    
        public void StartListening(string eventName, UnityAction<BaseEventData> listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
        }

        public void StopListening(string eventName, UnityAction<BaseEventData> listener)
        {
            if (Instance == null) return;
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public void TriggerEvent(string eventName, BaseEventData arg = null)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke(arg);
            }
        }

        public void ResetAllEvents()
        {
            _eventDictionary.Clear();
        }
    }
}