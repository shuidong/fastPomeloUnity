using UnityEngine;
using System.Collections;

public class RoundBall : MonoBehaviour {

    GameObject obj;
    float roundV = 60f;
	// Use this for initialization
	void Start () {
        this.obj = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    this.obj.transform.RotateAround(this.obj.transform.position, Vector3.up, roundV * Time.deltaTime);
    }
}
