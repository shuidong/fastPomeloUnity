using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onClick() {
        Debug.Log("btn click.");
        NetworkMgr.getInstance().newSocket();
    }
}
