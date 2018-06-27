using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    /*
     【カメラ移動クラス】
     位置はMainCameraオブジェクトのInspector上にあるoffsetにて変更可能
    */
    MapLoad mapLoad; // マップ読み込みクラス
    Player player; // プレイヤークラス

    public Transform playerTrans; // 追従する対象
    public Transform targetGimmick; // イベント時に追従する対象

    public Vector3 offset;  // カメラ位置の微調整用

    [SerializeField, Range(0, 600)] uint frameLimit; // ターゲットまでの所要移動時間（フレーム）
    [SerializeField] uint frameCount,endCount; // 現在の経過フレーム数
    [SerializeField] bool endFlg = false; // イベントカメラ移動の復路判定
    public bool eventFlg = false; // イベントが発生しているか

    [SerializeField] Vector3 velocity,returnVelocity; // イベント時のカメラ移動量
    public Vector3 topLeft; // カメラ左端の座標
    public Vector3 buttomRight; // カメラ右端の座標
    [SerializeField] Vector3 cameraMovePos; // カメラ移動量

    public float mapStartX,mapEndX; // マップの端点を保存する変数
    [SerializeField] float edgeStartX, edgeEndX; // 自キャラとカメラ端点の差分を保存する変数
    [SerializeField] float height,heightOffset; // ステージの高さ、カメラの高さ、高さの補正値
    // Use this for initialization
	void Start () {
        mapLoad = FindObjectOfType<MapLoad>(); // マップ読み込みクラスのコンポーネント取得
        player = FindObjectOfType<Player>(); // プレイヤークラスのコンポーネント取得
        playerTrans = player.transform; // プレイヤーの座標取得
        topLeft = GetTopLeft(); // カメラの左端を取得
        buttomRight = GetButtomRight(); // カメラの右端を取得
        mapStartX = 0; // マップの左端は必ず0になっている
        height = mapLoad.height; // マップ読み込みクラスよりマップの高さを取得する
        heightOffset = 5.5f; // 高さの補正値を設定
        mapEndX = Mathf.Max(mapLoad.width); //マップの右端を取得
        edgeStartX = Mathf.Abs(mapStartX - buttomRight.x + 0.5f); // マップ左端と自キャラの差分を求める
        edgeEndX = Mathf.Abs(mapEndX - edgeStartX); // マップ右端と自キャラの差分を求める
        // カメラ初期座標の設定
        this.transform.position = new Vector3(edgeStartX, playerTrans.position.y + offset.y, playerTrans.position.z + offset.z);
    }
	
	// UpDateメソッド終了後に呼び出し
	void LateUpdate () {
        if (Goal.clearFlg)
        {
            return;
        }
        topLeft = GetTopLeft(); // カメラ左端座標を取得
        buttomRight = GetButtomRight();// カメラ右端座標を取得
        // デバッグ用、F1キーが押されたらイベントカメラに切り替え
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    targetGimmick = GameObject.Find("Event1").GetComponent<Transform>();
        //    velocity = MoveSmooth(playerTrans.position, targetGimmick.position, frameLimit);
        //    eventFlg = true;
        //}
        //// イベントモードでなければ通常通りプレイヤーの移動に応じてカメラが移動
        //if (eventFlg)
        //{
        //    StartCoroutine("EventMove");
        //}

        // カメラ移動量はプレイヤーの座標より取得
        cameraMovePos = playerTrans.position;

        // マップ両端の座標と自キャラの差分の間のみ移動可能
        cameraMovePos.x = Mathf.Clamp(cameraMovePos.x, edgeStartX, edgeEndX);
        // マップにより設定された範囲内のみで移動可能
        cameraMovePos.y = Mathf.Clamp(cameraMovePos.y,-height + heightOffset,0);
        cameraMovePos.z = cameraMovePos.z + offset.z;

        // 計算後、カメラを移動する
        this.transform.position = cameraMovePos;

    }

    // イベントカメラ初期設定メソッド、目標地点のGameObjectを引数として使う
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

    // イベントカメラモードの距離を計算するメソッド
    // スタート地点から目標地点まで何秒で移動するかを計算
    // MoveSmooth（第一引数：移動開始座標点、第二引数：移動終了座標点、第三引数：移動時間（単位：フレーム））
    static Vector3 MoveSmooth(Vector3 startPos, Vector3 endPos, uint frame)
    {
        return new Vector3((endPos.x - startPos.x) / (float)frame,
            (endPos.y - startPos.y) / (float)frame,
            (endPos.z - startPos.z) / (float)frame);
    }

    // カメラ左端取得メソッド
    private Vector3 GetTopLeft()
    {
        topLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topLeft.Scale(new Vector3(1.0f, -1.0f, 1.0f));
        return topLeft;
    }

    // カメラ右端取得メソッド
    private Vector3 GetButtomRight()
    {
        buttomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0));
        buttomRight.Scale(new Vector3(1.0f, -1.0f, 1.0f));
        return buttomRight;
    }
}
