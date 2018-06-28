using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    // クリア判定
    public static bool clearFlg;
    // 木が切られた判定
    [SerializeField]
    public bool isBreak;
    // 水の上かどうか
    bool onWater;

    // クリア表示
    public static GameObject clearObj;

    // クリア判定用オブジェクト
    GameObject player;

	// Use this for initialization
	void Start () {
        clearFlg = false;
        isBreak = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 水の上以外はクリア判定をしない
        if (!onWater)
        {
            return;
        }

        // Playerが取得できているなら実行
        if(player != null && clearFlg == false)
        {
            // Playerが上に乗ったら実行
            if(player.transform.position.x >= transform.position.x)
            {
                player.transform.parent = gameObject.transform;
                clearFlg = true;
            }
        }

        // ゲームクリア判定がされていたら実行
        if (clearFlg)
        {
            // 画面外まで移動していく
            transform.position += new Vector3(2, 0, 0) * Time.deltaTime;
        }
	}

    void OnCollisionEnter(Collision hit)
    {
        // 水の上かどうかの判定
        if(hit.gameObject.name == "Water")
        {
            onWater = true;
        }
        // Playerを取得
        if(hit.gameObject.name == "Character")
        {
            player = hit.gameObject;
        }
    }

    // カメラから見えなくなったら実行
    void OnBecameInvisible()
    {
        // クリアしていないなら実行しない
        if(clearFlg == false)
        {
            return;
        }

        // カメラ外に移動し終わったらシーン移動
        if (clearFlg)
        {
            clearObj.SetActive(true);
        }
        //StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
        //SceneManager.LoadScene("GameClear");
    }
}
