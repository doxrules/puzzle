using UnityEngine;

namespace UnityTemplateProjects.Events
{

    public class TouchEvent : BaseEvent
    {
        public static string EventName = "TouchEvent";

        public override string GetEventName()
        {
            return EventName;
        }
    }

    public class TouchEventData : BaseEventData
    {
        public TouchManager.TouchState TouchState;
        public Vector2 DeltaIncrement;
        public float DeltaIncrementMagnitude;
        public Vector2 InitPosition;
        public Vector2 CurPosition;
    }

}