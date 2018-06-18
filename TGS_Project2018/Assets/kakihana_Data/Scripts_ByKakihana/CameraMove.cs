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
    [SerializeField, Range(0, 600)] uint frameLimit; // ターゲットまでの所要移動時間（フレーム）
    [SerializeField] uint frameCount,endCount; // 現在の経過フレーム数
    [SerializeField] bool endFlg = false; // イベントカメラ移動の復路判定

    public bool eventFlg = false; // イベントが発生しているか

    [SerializeField] Vector3 velocity,returnVelocity; // イベント時のカメラ移動量

	// Use this for initialization
	void Start () {
    }
	
	// UpDateメソッド終了後に呼び出し
	void LateUpdate () {
        // デバッグ用、F1キーが押されたらイベントカメラに切り替え
        if (Input.GetKeyDown(KeyCode.F1))
        {
            targetGimmick = GameObject.Find("Event1").GetComponent<Transform>();
            velocity = MoveSmooth(playerTrans.position, targetGimmick.position, frameLimit);
            eventFlg = true;
        }
        // イベントモードでなければ通常通りプレイヤーの移動に応じてカメラが移動
        if (eventFlg)
        {
            StartCoroutine("EventMove");
        }
        else
        {
            this.transform.position = playerTrans.position + offset;
        }
        
	}

    public void EventCamera(GameObject targetObj)
    {
        targetGimmick = targetObj.GetComponent<Transform>();
        velocity = MoveSmooth(playerTrans.position, targetGimmick.position, frameLimit);
        eventFlg = true;
    }

    // イベントモードカメラ移動
    IEnumerator EventMove()
    {
        if (frameCount < frameLimit && endFlg == false) // 設定された最大フレームになるまで
        {
            transform.position = transform.position + velocity; // 上下移動量を元に移動
            frameCount++; // フレームをカウント
            endCount = 0;
        }
        // 復路カメラ移動初期設定
        else if (frameCount >= frameLimit == endFlg == false)
        {
            yield return new WaitForSeconds(2.0f); // 2秒待つ
            endFlg = true;
            returnVelocity = -velocity; // 移動量を負に変える
            frameCount = 0;
        }
        // 復路カメラ移動
        if (endCount < frameLimit && endFlg == true)
        {
            transform.position = transform.position + returnVelocity;
            endCount++;
        }
        else if (endCount >= frameLimit && endFlg == true)
        {
            eventFlg = false;
            endFlg = false;
        }

    }

    static Vector3 MoveSmooth(Vector3 startPos, Vector3 endPos, uint frame)
    {
        return new Vector3((endPos.x - startPos.x) / (float)frame,
            (endPos.y - startPos.y) / (float)frame,
            (endPos.z - startPos.z) / (float)frame);
    }

}
