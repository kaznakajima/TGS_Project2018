using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using DG.Tweening;


// ----------------スクリーンショットをとるクラス-----------------------------------
// ※参照の仕方
// SingletonMonoBehaviour<ScreenShot>.Instance.変数名(またはメソッド)
//----------------------------------------------------------------------------------------

// どのシーンでも必要なのでシーンを変えても残るようにしてある。
// どのシーン内でもいるためステージの名前などを変数として持たせてある。
public class ScreenShot : SingletonMonoBehaviour<ScreenShot>
{
    // スクリーンショット
    [HideInInspector]
    public Texture2D tex2D;

    // 現在のステージ名
    [HideInInspector]
    public string csvName;

    // ステージ名のデータ
    public string[] csvData = { "1-1", "Test_2", "Test_3"};

    // 現在のシーンのキャンバス
    [HideInInspector]
    public GameObject currentCnavas;

    // 現在のステージ数の取得
    [HideInInspector]
    public int stageNum;

    // 自身のAudioSource
    [HideInInspector]
    public AudioSource myAudio;
    [HideInInspector]
    public AudioSource bgmAudio;

    // Playerを取得
    public Player[] GetPlayer()
    {
        return FindObjectsOfType<Player>().ToArray();
    }

    // Mirrorを取得
    public Mirror[] GetMirror()
    {
        return FindObjectsOfType<Mirror>().ToArray();
    }

    // ゴール判定を持つオブジェクトの取得
    public Goal[] GetBreakForest()
    {
        return FindObjectsOfType<Goal>().ToArray();
    }

    public AudioSource GetBGM()
    {
        return bgmAudio = GameObject.Find("BGM").gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update () {
		
	}

    // 写真の保存
    public IEnumerator SceneChangeShot()
    {
        yield return new WaitForEndOfFrame();

        // 現在の状態をスクリーンショットにする
        tex2D = new Texture2D(Screen.width, Screen.height);
        tex2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex2D.Apply();

        if(SceneManager.GetActiveScene().name == "Stage1_alpha" )
        {
            GameMaster master = FindObjectOfType<GameMaster>();
            if(master.sketchBookValue >= 0)
            {
                myAudio.PlayOneShot(myAudio.clip);
            }
            SingletonMonoBehaviour<ResetController>.Instance.CheckReset();
        }
    }
}
