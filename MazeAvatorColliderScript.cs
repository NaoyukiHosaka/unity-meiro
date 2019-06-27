using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeAvatorColliderScript : MonoBehaviour {
    public bool collisionFlg;
	// Use this for initialization
	void Start () {
        collisionFlg = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider collider)
    {
        MazeAppScript appscript = Camera.main.GetComponent<MazeAppScript>();
        if (appscript.IsEnd()) { return; }
        collisionFlg = true;
        appscript.LossPower(1);
    }
    private void OnTriggerExit(Collider other)
    {
        collisionFlg = false;
    }
}
