using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    /*
     【カメラ移動クラス】
     位置はMainCameraオブジェクトのInspector上にあるoffsetにて変更可能
    */

    public Transform playerTrans; // 追従する対象
    public Vector3 offset;  // カメラ位置の微調整用

	// Use this for initialization
	void Start () {
    }
	
	// UpDateメソッド終了後に呼び出し
	void LateUpdate () {
        this.transform.position = playerTrans.position + offset;
	}
}
