using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // タイトルのアニメーション
    Animator titleAnim;

	// Use this for initialization
	void Start () {
        titleAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Click"))
        {
            titleAnim.SetBool("Title_Flg", true);
        }
	}
}
