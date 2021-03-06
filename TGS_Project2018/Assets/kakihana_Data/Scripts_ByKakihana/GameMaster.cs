﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameMaster : MonoBehaviour {

    MapLoad mapLoad; // 残機を参照するステージクラス
    PageChange pc;
    Player player;

    const int BOOK_MAX_SIZE = 3;
    public GameObject[] bookValueObj = new GameObject[BOOK_MAX_SIZE];

    public int sketchBookValue; // 残りページ数
    public int tempSketchValue; // 差分用一時保存変数
    public Vector3 savePositon; // リスポーン用の座標を格納

    [SerializeField]
    Text lifeValue;

    [SerializeField] string changePageName;

	// Use this for initialization
	void Start () {
        Goal.clearObj = GameObject.Find("GameClear_Canvas");
        Goal.clearObj.SetActive(false);
        player = FindObjectOfType<Player>();
        // 現在のシーンの名前を取得
        mapLoad = GameObject.Find("StageInit").GetComponent<MapLoad>();
        pc = GameObject.FindObjectOfType<PageChange>(); // ページ遷移のコンポーネント取得
        sketchBookValue = BOOK_MAX_SIZE;
        tempSketchValue = sketchBookValue; // 差分用変数の値を設定
        for (int i = 0; i > bookValueObj.Length; i++) // 残機UIの初期設定
        {
            bookValueObj[i].GetComponent<GameObject>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckLife();
        // 現在の残りページと差分が異なれば残りページが減ったとみなす
        if (sketchBookValue != tempSketchValue && pc.pageChange == true)
        {
            Debug.Log("スケッチブック消費");
            lifeValue.text = string.Format(sketchBookValue.ToString());
            //bookValueObj[sketchBookValue].SetActive(false); // 残機UIを減らす
            tempSketchValue = sketchBookValue; // イベント後、再度値がおなじになるように設定
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

    void CheckLife()
    {
        if (sketchBookValue != 0)
        {
            return;
        }
        if (sketchBookValue == 0 && pc.pageFlip <= -1)
        {
            sketchBookValue = -1;
            StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
            {
                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                SceneManager.LoadScene("GameOver");
            });

        }
    }
}
