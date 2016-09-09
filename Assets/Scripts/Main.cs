using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    Game game;

	void Start () {
        GameObject go = this.gameObject;
        DontDestroyOnLoad(go);
        this.game = new Game();
        this.game.init();
    }

    void Update() {
        game.update();
    }
}
