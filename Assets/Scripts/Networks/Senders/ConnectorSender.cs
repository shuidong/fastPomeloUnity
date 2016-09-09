using UnityEngine;
using System.Collections;
using SimpleJson;

public class ConnectorSender {

    public static void sendEnter() {
        NetworkMgr.getInstance().request(Routes.CONNECTOR_ENTER, new JsonObject());
    }
}
