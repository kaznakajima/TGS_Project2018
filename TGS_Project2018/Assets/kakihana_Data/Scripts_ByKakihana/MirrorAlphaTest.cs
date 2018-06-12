using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAlphaTest : MonoBehaviour {

    GameObject playerObj;
    Transform playerTrans,myTrans; // 自分の座標とプレイヤーの座標を格納
    [SerializeField] GameObject spriteObj; // マスクするオブジェクト
    public SpriteRenderer sr; // 参照するSpriteRenderer
    [SerializeField] SpriteRenderer[] elementList = new SpriteRenderer[4];
   [SerializeField] float alpha,time,fadeTime = 1.0f; // 現在アルファ値、経過時間、完全に変化するまでの時間
    float speed = 0.02f; // 変化時間
    [SerializeField] float alphaJudge;
    public bool alphaFlg = false;

    [SerializeField] Vector3 pos;

	// Use this for initialization
	void Start () {
        alpha = 0.0f; // アルファ値の初期設定
        time = fadeTime; // フェード時間の設定
        playerObj = GameObject.FindGameObjectWithTag("Player"); // プレイヤーの座標を取得
        //playerTrans = playerObj.GetComponent<Transform>();
        myTrans = this.gameObject.GetComponent<Transform>(); // 自分の座標を取得
        sr = spriteObj.GetComponent<SpriteRenderer>(); // 非表示にしたいSprite
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha); // Spriteの初期設定
        elementList = playerObj.GetComponent<Player>().myElement;
        foreach (var icon in elementList)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        }
        pos = playerObj.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        pos = playerObj.transform.position;
        alphaJudge = Mathf.Abs(myTrans.position.x - pos.x);
        playerTrans = playerObj.GetComponent<Transform>();
        if (alphaFlg == true)
        {
            StartAlpha();
        }
        else
        {
            EndAlpha();
        }
        // 自分とキャラクター間座標の差の絶対値が３以下だったら
        if (Mathf.Abs(alphaJudge/*playerTrans.position.x*/) <= 3)
        {

        }// 自分とキャラクター間座標の差の絶対値が３以上だったら
        else if (Mathf.Abs(alphaJudge/*playerTrans.position.x*/) >= 3)
        {

        }
	}
    public void StartAlpha()
    {
        // フェード実行、Srを非表示に
        time -= Time.deltaTime;
        alpha += speed;
        if (alpha >= 1.0f)
        {
            alpha = 1.0f;
        }
        var color = sr.color;
        // 新しいα値を設定
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        foreach (var icon in elementList)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        }
    }
    public void EndAlpha()
    {
        Debug.Log("kenti");
        // フェードを実行し、もとに戻す
        alpha -= speed;
        if (alpha <= -1.0f)
        {
            alpha = -1.0f;
        }
        var color = sr.color;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        foreach (var icon in elementList)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        }
    }

}
