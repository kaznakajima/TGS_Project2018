using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move : StatusController
{
    [SerializeField]
    float speed;

    Vector3 inputVec;

    // プレイヤー用Canvas
    [SerializeField]
    GameObject canvasTrans;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        canvasTrans.transform.position = new Vector3(transform.position.x, transform.position.y, canvasTrans.transform.position.z);

        float moveX = Input.GetAxisRaw("Horizontal");
        if(moveX > 0)
        {
            inputVec.x = 1;
            PlayerMove(inputVec.x);
        }
        else if(moveX < 0)
        {
            inputVec.x = -1;
            PlayerMove(inputVec.x);
        }
        if(moveX == 0)
        {
            inputVec.x = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            status = STATUS.NONE;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            status = STATUS.FIRE;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            status = STATUS.WATER;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            status = STATUS.WIND;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            status = STATUS.EARTH;
        }
    }

    void PlayerMove(float inputVec)
    {
        transform.position += new Vector3(inputVec, 0, 0)  * speed * Time.deltaTime;
    }

    // ステータスに応じて姿を変化させるメソッド
    // 引数　現在のステータス
    public override void StatusChenge(STATUS _status)
    {
        SpriteRenderer playerSp = gameObject.GetComponent<SpriteRenderer>();
    }

    // 姿を変える
    //IEnumerator FormeChange()
    //{
    //    yield return new WaitForSeconds(3.0f);

    //    StatusChenge(status);

    //    canvasTrans.transform.DOScale(new Vector3(0.0f, 2.5f, 1.0f), 1.0f).OnComplete(() =>
    //      {
    //          canvasTrans.transform.DOScale(new Vector3(1.0f, 2.5f, 1.0f),1.0f);
    //      });
    //}
}
