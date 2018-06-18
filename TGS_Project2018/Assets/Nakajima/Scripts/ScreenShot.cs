using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [HideInInspector]
    public string[] csvData = { "1-1", "Test_2", "1-2_2" };

    // 現在のシーンのキャンバス
    [HideInInspector]
    public GameObject currentCnavas;

    // 現在のステージ数の取得
    [HideInInspector]
    public int stageNum;

    // 自身のAudioSource
    AudioSource myAudio;

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

        myAudio.PlayOneShot(myAudio.clip);
    }
}
