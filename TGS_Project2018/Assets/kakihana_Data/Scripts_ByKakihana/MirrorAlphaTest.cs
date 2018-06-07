using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAlphaTest : MonoBehaviour {

    Transform playerTrans,myTrans;
    [SerializeField] GameObject spriteObj;
    public SpriteRenderer sr;
   [SerializeField] float alpha,time,fadeTime = 1.0f;
    float speed = 0.02f;

	// Use this for initialization
	void Start () {
        alpha = 0.0f;
        time = fadeTime;
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTrans = this.gameObject.GetComponent<Transform>();
        sr = spriteObj.GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(myTrans.position.x - playerTrans.position.x) <= 3)
        {
            Debug.Log("kenti1");
            time -= Time.deltaTime;
            alpha += speed;
            if (alpha >= 1.0f)
            {
                alpha = 1.0f;
            }
            var color = sr.color;
            sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,alpha);
        }
        else if (Mathf.Abs(myTrans.position.x - playerTrans.position.x) >= 3)
        {
            Debug.Log("kenti2");
            alpha -= speed;
            if (alpha <= -1.0f)
            {
                alpha = -1.0f;
            }
            var color = sr.color;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
	}
}
