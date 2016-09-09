using UnityEngine;
using System.Collections;

public class Game {
    NetworkMgr networkMgr;

    public Game() {
    }

    public void init() {
        networkMgr = NetworkMgr.getInstance();
        
    }

    // Update is called once per frame
    public void update () {
        this.networkMgr.update();
	}
}
