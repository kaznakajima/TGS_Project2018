using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingJudgment : MonoBehaviour {

    Player player;

    [SerializeField] Transform rayPos;
    [SerializeField] float rayRange = 1.0f;
    float fallPos;
    bool fallingFlg;
    float fallDistance;
    [SerializeField] float fallingDamageDistance = 4.0f;


    // Use this for initialization
    void Start () {
        fallDistance = 0.0f;
        fallPos = this.transform.position.y;
        fallingFlg = false;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(rayPos.position, rayPos.position + Vector3.down * rayRange, Color.blue);
        if (fallingFlg == true)
        {
            fallPos = Mathf.Max(fallPos, this.transform.position.y);
            Debug.Log(fallPos);
            if (Physics.Linecast(rayPos.position, rayPos.position + Vector3.down * rayRange, LayerMask.GetMask("Ground")))
            {
                fallDistance = fallPos - this.transform.position.y;
            }
        }
        else
        {

        }

    }
}
