using System;
using UnityEngine.Events;

namespace UnityTemplateProjects.Events
{

    [Serializable]
    public abstract class BaseEvent : UnityEvent<BaseEventData>
    {
        public void Initialize()
        {
            EventManager.Instance.RegisterEvent(this);
        }

        public abstract string GetEventName();

        public void Raise(BaseEventData data)
        {
            EventManager.Instance.TriggerEvent(GetEventName(), data);
        }
    }

    public interface BaseEventData { }

}