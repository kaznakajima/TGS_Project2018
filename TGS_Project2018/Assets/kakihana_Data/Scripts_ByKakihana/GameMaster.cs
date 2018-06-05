using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public int sketchBookValue; // 残りページ数
    string stageNo; // ステージ番号
    int tempSketchValue; // 差分用一時保存変数

	// Use this for initialization
	void Start () {
        // 現在のシーンの名前を取得
        stageNo = SceneManager.GetActiveScene().name;
        // ステージによって残りページを設定
        switch (stageNo)
        {
            case "Stage1":
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
	}
	
	// Update is called once per frame
	void Update () {
        // 現在の残りページと差分が異なれば残りページが減ったとみなす
        if (sketchBookValue != tempSketchValue)
        {
            Debug.Log("スケッチブック消費");
            tempSketchValue = sketchBookValue; // イベント後、再度値がおなじになるように設定
        }
	}


}
