using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;

public class CallbackFact {
    public static string ROUTE_KEY_NAME = "__r_";
    private static Dictionary<string, Action<JsonObject>> callbacks = new Dictionary<string, Action<JsonObject>>();
    
    //注册各种 回调
    public static void registCallbacks() {
        registCallback(Routes.CONNECTOR_ENTER, ConnectorHandler.exec);
    }

    private static void registCallback(string route, Action<JsonObject> handler)
    {
        if (CallbackFact.callbacks.ContainsKey(route)) {//已经注册过了同名的
            Debug.LogError("already exist route: " + route);
            return;
        }
        CallbackFact.callbacks.Add(route, handler);
    }

    //执行回调
    public static void invokeHandle(string route, JsonObject data) {
        Action<JsonObject> handler;
        CallbackFact.callbacks.TryGetValue(route, out handler);
        if(handler != null)
        {
            handler(data);
        }else
        {
            Debug.LogWarning("cannot find handler for " + route);
        }
    }
}
