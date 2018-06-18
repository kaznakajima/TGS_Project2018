using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    MapLoad mapLoad; // 残機を参照するステージクラス
    PageChange pc;
    Player player;

    public Image[] bookValue =  new Image[5]; // 残り残機UI

    public int sketchBookValue; // 残りページ数
    //string stageNo; // ステージ番号
    public int tempSketchValue; // 差分用一時保存変数
    public Vector3 savePositon;

    [SerializeField] string changePageName;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        // 現在のシーンの名前を取得
        mapLoad = GameObject.Find("StageInit").GetComponent<MapLoad>();
        pc = GameObject.Find(changePageName).GetComponent<PageChange>(); // ページ遷移のコンポーネント取得
        // ステージによって残りページを設定
        switch (mapLoad.CSVName)
        {
            case "1-1":
                sketchBookValue = 5;
                break;
            case "Test":
                sketchBookValue = 3;
                break;
            case "Test_2":
                sketchBookValue = 3;
                break;
            default:
                sketchBookValue = 5;
                break;
        }
        tempSketchValue = sketchBookValue; // 差分用変数の値を設定
        for (int i = bookValue.Length ; i <bookValue.Length - sketchBookValue; i--) // 残機UIの初期設定
        {
            bookValue[i].enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        // 現在の残りページと差分が異なれば残りページが減ったとみなす
        if (sketchBookValue != tempSketchValue && pc.pageChange == true)
        {
            Debug.Log("スケッチブック消費");
            bookValue[sketchBookValue].enabled = false; // 残機UIを減らす
            tempSketchValue = sketchBookValue; // イベント後、再度値がおなじになるように設定
        }
        else if (sketchBookValue == -1 && pc.pageChange == false)
        {
            StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
            SceneManager.LoadScene("GameOverScene");
        }
	}

    public void SavePosition(Vector3 pos)
    {
        savePositon = pos;
    }

    public Vector3 GetPosition()
    {
        return savePositon;
    }


}
