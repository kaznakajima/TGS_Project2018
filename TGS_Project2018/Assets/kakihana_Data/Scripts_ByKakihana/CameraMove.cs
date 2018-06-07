using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    /*
     【カメラ移動クラス】
     位置はMainCameraオブジェクトのInspector上にあるoffsetにて変更可能
    */

    public Transform playerTrans; // 追従する対象
    public Transform targetGimmick; // イベント時に追従する対象
    public Vector3 offset;  // カメラ位置の微調整用
    [SerializeField, Range(0, 600)] uint frameLimit;
    [SerializeField] uint frameCount,endCount;
    [SerializeField] bool endFlg = false;

    public bool eventFlg = false;

    [SerializeField] Vector3 velocity,returnVelocity;

	// Use this for initialization
	void Start () {
    }
	
	// UpDateメソッド終了後に呼び出し
	void LateUpdate () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            targetGimmick = GameObject.Find("Event1").GetComponent<Transform>();
            velocity = MoveSmooth(playerTrans.position, targetGimmick.position, frameLimit);
            eventFlg = true;
        }

        if (eventFlg)
        {
            StartCoroutine("EventMove", 1);
        }
        else
        {
            this.transform.position = playerTrans.position + offset;
        }
        
	}

    IEnumerator EventMove(int stateNum)
    {
        switch (stateNum)
        {
            case 1:
                if (frameCount < frameLimit && endFlg == false) // 設定された最大フレームになるまで
                {
                    transform.position = transform.position + velocity; // 上下移動量を元に移動
                    frameCount++; // フレームをカウント
                }
                else if(frameCount >= frameLimit == endFlg == false)
                {
                    yield return new WaitForSeconds(2.0f);
                    endFlg = true;
                    returnVelocity = -velocity;
                    frameCount = 0;
                }
                if (endCount < frameLimit && endFlg == true)
                {
                    transform.position = transform.position + returnVelocity;
                    endCount++;
                }
                else if(endCount >= frameLimit && endFlg == true)
                {
                    eventFlg = false;
                    endFlg = false;
                }
                break;
            default:
                break;
        }
        
    }

    static Vector3 MoveSmooth(Vector3 startPos, Vector3 endPos, uint frame)
    {
        return new Vector3((endPos.x - startPos.x) / (float)frame,
            (endPos.y - startPos.y) / (float)frame,
            (endPos.z - startPos.z) / (float)frame);
    }

}
