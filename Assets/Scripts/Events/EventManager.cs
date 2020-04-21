namespace UnityTemplateProjects.Events
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;
    using UnityTemplateProjects.Events;

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
                    _instance = new EventManager();
                    _instance._eventDictionary = new Dictionary<string, BaseEvent>();
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
            BaseEvent thisEvent = null;
            if (_eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
        }

        public void StopListening(string eventName, UnityAction<BaseEventData> listener)
        {
            if (Instance == null) return;
            BaseEvent thisEvent = null;
            if (_eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public void TriggerEvent(string eventName, BaseEventData arg = null)
        {
            BaseEvent thisEvent = null;
            if (_eventDictionary.TryGetValue(eventName, out thisEvent))
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