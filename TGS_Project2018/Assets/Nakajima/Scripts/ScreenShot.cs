using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : SingletonMonoBehaviour<ScreenShot>
{
    // スクリーンショット
    [HideInInspector]
    public Texture2D tex2D;

    // ステージ名
    [HideInInspector]
    public string csvName;

    // 現在のシーンのキャンバス
    [HideInInspector]
    public GameObject currentCnavas;

    void Start()
    {
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
    }
}
