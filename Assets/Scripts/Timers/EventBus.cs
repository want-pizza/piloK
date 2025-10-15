using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<string, Action> eventListeners = new Dictionary<string, Action>();

    public static void Subscribe(string eventName, Action callback)
    {
        if (!eventListeners.ContainsKey(eventName))
            eventListeners[eventName] = callback;
        else
            eventListeners[eventName] += callback;
    }

    public static void Unsubscribe(string eventName, Action callback)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] -= callback;
            if (eventListeners[eventName] == null)
                eventListeners.Remove(eventName);
        }
    }

    public static void Publish(string eventName)
    {
        if (eventListeners.ContainsKey(eventName))
            eventListeners[eventName]?.Invoke();
    }
}
