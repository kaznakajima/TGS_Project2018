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
    // リセットアイコン表示フラグ
    public static bool resetIsonFlg = false;

    // リセット(生成)可能かどうか
    [HideInInspector]
    public bool canReset;

    // 木が元々あった位置
    [HideInInspector]
    public Vector3 TreePos;
    // ツタが元々あった位置、ツタのオブジェクト
    [HideInInspector]
    public Vector3 IvyPos;
    [HideInInspector]
    public GameObject IvyObj;

    // リセット用のオブジェクト
    [SerializeField]
    GameObject mirrorObj; // 鏡
    [SerializeField]
    GameObject TreeObj; // 木
    [SerializeField]
    GameObject GoalObj; // ゴール用オブジェクト

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
            Instantiate(GoalObj, new Vector3((int)TreePos.x, (int)TreePos.y, 0.0f), Quaternion.identity);
            Instantiate(mirrorObj, new Vector3((int)TreePos.x - 1, (int)TreePos.y, 0.0f), Quaternion.identity);
            foreach(var player in SingletonMonoBehaviour<ScreenShot>.Instance.GetPlayer())
            {
                if(Mathf.Abs(player.transform.position.x - (int)TreePos.x) <= 3)
                {
                    player.changeFlg = true;
                    player.FormChange((int)ANIM_ENUMS.BLUCK.IDLE, StatusController.STATUS.NONE);
                    player.transform.position = new Vector3((int)TreePos.x - 2, (int)TreePos.y, 0.0f);
                }
            }
            resetIsonFlg = false;
            canReset = false;
        }

        // ツタが登り切らなかったらリセット
        if(IvyObj != null)
        {
            IvyObj.transform.position = IvyPos;
            IvyObj.GetComponent<RainGimmick>().isHit = false;
            foreach (var player in SingletonMonoBehaviour<ScreenShot>.Instance.GetPlayer())
            {
                if (Mathf.Abs(player.transform.position.x - (int)IvyObj.transform.position.x) <= 1)
                {
                    player.changeFlg = true;
                    player.FormChange((int)ANIM_ENUMS.BLUCK.IDLE, StatusController.STATUS.NONE);
                    player.transform.position = new Vector3((int)IvyObj.transform.position.x - 2, (int)IvyObj.transform.position.y + 2.0f, 0.0f);
                }
            }
            Instantiate(mirrorObj, new Vector3((int)IvyPos.x, (int)IvyPos.y + 1, 0.0f), Quaternion.identity);
            resetIsonFlg = false;
            canReset = false;
        }

        // 木が壊されているか確認
        foreach (Goal forest in SingletonMonoBehaviour<ScreenShot>.Instance.GetBreakForest())
        { 
            // 壊されているならオブジェクトもリセット
            if(forest.isBreak == true)
            {
                Instantiate(TreeObj, new Vector3((int)forest.gameObject.transform.position.x - 1,
                    (int)forest.gameObject.transform.position.y, 0.0f), Quaternion.identity);
                Instantiate(mirrorObj, new Vector3((int)forest.gameObject.transform.position.x - 2,
                    (int)forest.gameObject.transform.position.y, 0.0f), Quaternion.identity);

                foreach (var player in SingletonMonoBehaviour<ScreenShot>.Instance.GetPlayer())
                {
                    if (Mathf.Abs(player.transform.position.x - (int)forest.gameObject.transform.position.x) <= 2)
                    {
                        player.changeFlg = true;
                        player.FormChange((int)ANIM_ENUMS.BLUCK.IDLE, StatusController.STATUS.NONE);
                        player.transform.position = new Vector3((int)forest.gameObject.transform.position.x - 3,
                    (int)forest.gameObject.transform.position.y, 0.0f);
                    }
                }

                Destroy(forest.gameObject);
                canReset = false;
                resetIsonFlg = false;
            }
        }
    }

    // 鏡の姿だけリセット
    public void StatusReset()
    {
        foreach (Player player in SingletonMonoBehaviour<ScreenShot>.Instance.GetPlayer())
        {
            player.changeFlg = true;
            player.FormChange((int)ANIM_ENUMS.BLUCK.IDLE, StatusController.STATUS.NONE);
        }

        // Mirrorの取得
        foreach (Mirror mirror in SingletonMonoBehaviour<ScreenShot>.Instance.GetMirror())
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
            //canReset = false;
        }
        resetIsonFlg = false;
    }

	// Use this for initialization
	void Start () {
        canReset = false;
        resetIsonFlg = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (SingletonMonoBehaviour<ScreenShot>.Instance.GetBreakForest().Length == 0)
        {
            resetIsonFlg = true;
        }
    }
}
