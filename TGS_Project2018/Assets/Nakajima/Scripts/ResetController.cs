using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------リセットをするクラス-----------------------------------
// ※参照の仕方
// SingletonMonoBehaviour<ResetController>.Instance.変数名(またはメソッド)
//----------------------------------------------------------------------------------------

// ゲームシーンのみで使用
public class ResetController : SingletonMonoBehaviour<ResetController>
{
    // リセット可能かどうか
    [HideInInspector]
    public bool canReset;

    // 木が元々あった位置
    [HideInInspector]
    public Vector3 TreePos;

    // リセット用のオブジェクト
    [SerializeField]
    GameObject mirrorObj; // 鏡
    GameObject resetMirror;
    [SerializeField]
    GameObject TreeObj; // 木
    GameObject resetTree;

    // オブジェクトのリセット
    public void CheckReset()
    {
        // Mirrorの取得
        foreach (Mirror mirror in SingletonMonoBehaviour<ScreenShot>.Instance.GetMirror())
        {
            // ギミック作動中なら変化できない
            if (mirror.isGimmick)
            {
                return;
            }
            // 鏡のステータスが通常じゃなければ姿だけリセット
            if ((int)mirror.status != 0 && !mirror.isGimmick)
            {
                StatusReset();
            }
        }

        // リセットフラグが false ならリターン
        if (!canReset)
        {
            return;
        }

        // 燃やされてもリセット
        if (TreePos != Vector3.zero)
        {
            Instantiate(TreeObj, new Vector3((int)TreePos.x,(int)TreePos.y, 0.0f), Quaternion.identity);
            Instantiate(mirrorObj, new Vector3((int)TreePos.x - 1, (int)TreePos.y, 0.0f), Quaternion.identity);
            canReset = false;
        }

        // 木が壊されているか確認
        foreach (Goal forest in SingletonMonoBehaviour<ScreenShot>.Instance.GetBreakForest())
        {
            // リセット済みならリターン
            if(resetMirror != null || resetTree != null)
            {
                return;
            }

            // 壊されているならオブジェクトもリセット
            if(forest.isBreak == true)
            {
                Instantiate(TreeObj, new Vector3((int)forest.gameObject.transform.position.x,
                    (int)forest.gameObject.transform.position.y, 0.0f), Quaternion.identity);
                Instantiate(mirrorObj, new Vector3((int)forest.gameObject.transform.position.x - 1,
                    (int)forest.gameObject.transform.position.y, 0.0f), Quaternion.identity);

                Destroy(forest.gameObject);
                canReset = false;
            }
        }
    }

    // 鏡の姿だけリセット
    public void StatusReset()
    {
        // Mirrorの取得
        foreach(Mirror mirror in SingletonMonoBehaviour<ScreenShot>.Instance.GetMirror())
        {
            // Mirrorのステータスを通常に変更
            mirror.StatusChenge(StatusController.STATUS.NONE);
            mirror.status = StatusController.STATUS.NONE;
            mirror.mirrorObj.SetActive(true);
            if(mirror.rainObjInstance != null)
            {
                mirror.myAudio.Stop();
                Destroy(mirror.rainObjInstance);
            }
            canReset = false;
        }
    }

	// Use this for initialization
	void Start () {
        canReset = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
