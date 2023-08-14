using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoSingleton<EventManager>
{
    public Dictionary<string,UnityEvent> eventDictionary = null;

    public override void OnInitialize()
    {
        if(eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }
    
    public static void StartListening (string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        } 
        else
        {
            thisEvent = new UnityEvent ();
            thisEvent.AddListener (listener);
            instance.eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void TriggerEvent (string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Invoke ();
        }
    }

}
