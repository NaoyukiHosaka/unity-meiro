using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSphereScript : MonoBehaviour {
    private Color oldc;
    private int counter = 0;
	// 初期色の保管
	void Start () {
        oldc = GetComponent<Renderer>().material.color;
	}
	
	// カウンタ変数をチェックして色をもとに戻す、プレイヤーのいる方向に押す
	void FixedUpdate () {
        MazeAppScript appScript = Camera.main.GetComponent<MazeAppScript>();
        if (appScript.IsEnd()) { return; }
        Renderer renderer = GetComponent<Renderer>();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (counter > 0) {
            counter--;
            return;
        }
        else
        {
            renderer.material.color = oldc;
        }
        Vector3 v1 = GameObject.Find("unitychan").transform.position;
        Vector3 v2 = transform.position;
        if (rigidbody.velocity.magnitude < appScript.mazeLevel)
        {
            Vector3 vd = v1 - v2;
            vd /= vd.magnitude;
            rigidbody.AddForce(vd);
        }
	}
    //衝突してパワーダウン後しばらく停止
    void CollisionEnter(Collision collider)
    {
        MazeAppScript appscript = Camera.main.GetComponent<MazeAppScript>();
        if (appscript.IsEnd()) { return; }
        if (counter > 0) { return; }
        if (collider.gameObject.name == "unitychan")
        {
            Renderer renderer = GetComponent<Renderer>();
            oldc = renderer.material.color;
            renderer.material.color = new Color(0, 0, 1, 0.5f);
            counter = (int)(1000 / appscript.mazeLevel);
            appscript.LossPower(10);
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.red;
        }
    }
    //離れたら床のいろを戻す
    private void OnCollisionExit(Collision collider)
    {
        if (collider.gameObject.name == "unitychan")
        {
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.red;
        }
    }
    private void OnCollisionExit(Collision2D collider)
    {
        if (collider.gameObject.name == "unitychan")
        {
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.white;
            collider.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }
    }
}
