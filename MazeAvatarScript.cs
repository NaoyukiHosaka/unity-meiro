using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeAvatarScript : MonoBehaviour {
    Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        MazeAppScript appscript = Camera.main.GetComponent<MazeAppScript>();
        if (appscript.IsEnd())
        {
            animator.SetFloat("Speed", 0);
            animator.SetFloat("Direction", 0);
            animator.SetBool("Jump", false);
            return;
        }
        MazeAvatorColliderScript avatorColliderScript = GameObject.Find("unitychan").GetComponent<MazeAvatorColliderScript>();
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (avatorColliderScript.collisionFlg == false)
        {
            animator.SetFloat("Speed", v);
            animator.SetFloat("Direction", h);
            animator.SetBool("Jump", false);
            Vector3 vector = new Vector3(0, 0, v);
            vector = transform.TransformDirection(vector) * 5f;
            transform.localPosition += vector * Time.fixedDeltaTime;
            transform.Rotate(0, h, 0);
        }
        else
        {
            Vector3 vec = transform.forward;
            vec *= -1;
            vec.y = 0;
            transform.position += vec;
            Vector3 vec2 = new Vector3(0, 180, 0);
            transform.Rotate(vec2);
            avatarColliderscript.collisionFlg = false;
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }
	}
}
