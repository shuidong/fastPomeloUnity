using UnityEngine;
using System.Collections;
using SimpleJson;

public class GameHandler {

    public static void onRaceWait(JsonObject data) {
        Debug.Log("onRaceWait-: " + data);
    }
}
