using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;

public class BroadcastFact {
    public static string IS_EVT_KEY_NAME = "__evt_";
    public static Dictionary<string, Action<JsonObject>> callbacks = new Dictionary<string, Action<JsonObject>>();

    public static void registEvents()
    {
        BroadcastFact.callbacks.Add(Routes.ON_RACE_WAIT, GameHandler.onRaceWait);
    }

    public static void invokeHandle(string route, JsonObject data)
    {
        Action<JsonObject> handler;
        BroadcastFact.callbacks.TryGetValue(route, out handler);
        if (handler != null)
        {
            handler(data);
        }
        else
        {
            Debug.LogWarning("cannot find handler for " + route);
        }
    }
}
