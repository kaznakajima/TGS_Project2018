using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    MapLoad mapLoad; // 残機を参照するステージクラス

    public Image[] bookValue =  new Image[5]; // 残り残機UI

    public int sketchBookValue; // 残りページ数
    //string stageNo; // ステージ番号
    [SerializeField] int tempSketchValue; // 差分用一時保存変数

	// Use this for initialization
	void Start () {
        // 現在のシーンの名前を取得
        mapLoad = GameObject.Find("StageInit").GetComponent<MapLoad>();
        // ステージによって残りページを設定
        switch (mapLoad.CSVName)
        {
            case "1-1":
                sketchBookValue = 5;
                break;
            case "Stage2":
                sketchBookValue = 10;
                break;
            default:
                sketchBookValue = 20;
                break;
        }
        tempSketchValue = sketchBookValue; // 差分用変数の値を設定
        for (int i = 0; i < 5; i++) // 残機UIの初期設定
        {
            bookValue[i].enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        // 現在の残りページと差分が異なれば残りページが減ったとみなす
        if (sketchBookValue != tempSketchValue)
        {
            Debug.Log("スケッチブック消費");
            bookValue[sketchBookValue].enabled = false; // 残機UIを減らす
            tempSketchValue = sketchBookValue; // イベント後、再度値がおなじになるように設定
        }
	}


}
