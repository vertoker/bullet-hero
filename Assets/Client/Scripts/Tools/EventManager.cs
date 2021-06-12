using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public static class EventManager
{
    private static Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();

    public static void Add(string key, UnityEvent @event)
    {
        events.Add(key, @event);
    }
    public static void Remove(string key, UnityEvent @event)
    {
        events.Remove(key);
    }
    public static void Call(string key)
    {
        events[key].Invoke();
    }
    public static void AddListener(string key, UnityAction action)
    {
        events[key].AddListener(action);
    }
    public static void RemoveListener(string key, UnityAction action)
    {
        events[key].RemoveListener(action);
    }
}
