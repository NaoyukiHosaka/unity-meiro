using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewScript : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	//カメラの位置を調整
	void Update () {
        Vector3 vec = target.position;
        Vector3 fvec = target.forward;
        vec.y = 2.5f;
        fvec *= 4f;
        fvec.y = -1f;
        Camera.main.transform.position = vec - fvec;
        Camera.main.transform.LookAt(vec);
	}
}
