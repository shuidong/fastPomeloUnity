using UnityEngine;
using System.Collections;
using Pomelo.DotNetClient;
using System;
using SimpleJson;
using System.Collections.Generic;

public class NetworkMgr{

    private string host = "127.0.0.1";
    private int port = 3009;

    private PomeloClient pclient;
    private Queue<JsonObject> msgQueue;//消息队列（收到的）

    private static NetworkMgr _instance;
    public static NetworkMgr getInstance() {
        if (_instance == null) {
            _instance = new NetworkMgr();
        }
        return _instance;
    }

    public NetworkMgr() {
        this.msgQueue = new Queue<JsonObject>();

        //把回调的key和托管函数 对应起来
        CallbackFact.registCallbacks();
        BroadcastFact.registEvents();
    }

    //切断连接
    public void disconnect() {
        pclient.disconnect();
    }

    //建立起一条新的连接
    public void newSocket() {
        pclient = new PomeloClient();

        pclient.NetWorkStateChangedEvent += (state) =>
        {
            Debug.Log(state);
            //Console.WriteLine(state);
        };

        
        pclient.initClient(host, port, () =>
        {
            //The user data is the handshake user params
            JsonObject user = new JsonObject();
            pclient.connect(user, data =>
            {
                ConnectorSender.sendEnter();
                //pclient.request(Routes.CONNECTOR_ENTER, new JsonObject(), OnEnterConnector);
            });

            //把广播事件的名称都注册到pomelo中
            foreach (KeyValuePair<string, Action<JsonObject>> pair in BroadcastFact.callbacks)
            {
                this.registBroadcast(pair.Key);
            }
        });
    }

    //通知给对方服务器一个消息，不会有回传数据
    public void notify(string route, JsonObject message) {
        pclient.notify(route, message);
    }

    //请求消息，会有回调
    public void request(string route, JsonObject message) {
        pclient.request(route, message, (data) => {
            data[CallbackFact.ROUTE_KEY_NAME] = route;
            this.msgQueue.Enqueue(data);
        });
    }

    //注册了 广播事件的 处理逻辑
    public void registBroadcast(string eventName) {
        pclient.on(eventName, (data) => {
            data[CallbackFact.ROUTE_KEY_NAME] = eventName;
            data[BroadcastFact.IS_EVT_KEY_NAME] = true;//表示它是一个 广播事件，
            this.msgQueue.Enqueue(data);
        });
    }

    //扫描消息队列，看是否有数据要处理
    public void update() {
        while (this.msgQueue.Count > 0) {
            JsonObject data = this.msgQueue.Dequeue();
            string route = data[CallbackFact.ROUTE_KEY_NAME] as string;
            if(route == null)
            {
                Debug.LogError("cannt find route in msg");
            }
            //调用回调函数
            
            if (!data.ContainsKey(BroadcastFact.IS_EVT_KEY_NAME))
            {//不是广播
                CallbackFact.invokeHandle(route, data);
            }
            else {
                BroadcastFact.invokeHandle(route, data);
            }
        }
    }


}
