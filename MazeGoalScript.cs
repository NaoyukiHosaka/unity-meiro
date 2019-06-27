using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGoalScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(1, 1, 1));
    }
    private void OnTriggerEnter(Collider collider)
    {
        MazeAppScript appscript = Camera.main.GetComponent<MazeAppScript>();
        if (appscript.IsEnd()) { return; }
        if (collider.gameObject.name == "unitychan")
        {
            appscript.GoodEnd();
        }
    }
}
