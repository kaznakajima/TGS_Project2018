using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public Vector3 move;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += move * Time.deltaTime;
    }

    void OnCollisionEnter(Collision hit)
    {
        if(hit.gameObject.tag == "Player")
        {
            move.x += 1.0f;
            Debug.Log("検知");
        }
    }

}
