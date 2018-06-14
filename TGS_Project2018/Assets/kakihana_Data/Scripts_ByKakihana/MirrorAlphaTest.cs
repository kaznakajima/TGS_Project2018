using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAlphaTest : MonoBehaviour {

    public enum Mode
    {
        WAIT = 0,
        MONITOR,
        ALPHAON,
        ALPHAOFF,
        CHANGED
    }

    GameObject playerObj;
    Transform playerTrans,myTrans; // 自分の座標とプレイヤーの座標を格納
    [SerializeField] GameObject spriteObj; // マスクするオブジェクト
    public SpriteRenderer sr; // 参照するSpriteRenderer
    [SerializeField] SpriteRenderer[] elementList = new SpriteRenderer[4];
    [SerializeField] float alpha;// 現在アルファ値、経過時間、完全に変化するまでの時間
    float speed = 0.02f; // 変化時間
    [SerializeField] float alphaJudge,oldJudge;

    [SerializeField] Vector3 playerPos;

    [SerializeField] Mode actionMode;

    [SerializeField] bool changed;

	// Use this for initialization
	void Start () {
        AlphaSetup();
        ModeInit();
    }
	
	// Update is called once per frame
	void Update () {
        //if (alphaJudge != oldJudge)
        //{
        //    ModeInit();
        //}
        switch (actionMode)
        {
            case Mode.WAIT:
                WaitMode();
                break;
            case Mode.MONITOR:
                MonitorMode();
                break;
            case Mode.ALPHAOFF:
                do
                {
                    alpha += speed;
                    ModeInit();
                    AlphaOff();
                } while (alpha == 1.0f);
                changed = true;
                break;
            case Mode.ALPHAON:
                AlphaOn();
                changed = false;
                break;
            case Mode.CHANGED:
                AlphaLock(alpha);
                break;
        }
        //playerPos = playerObj.transform.position;
        //alphaJudge = Mathf.Abs(myTrans.position.x - playerPos.x);
        //if (alphaJudge > 3.0f && alpha != -1)
        //{
        //    EndAlpha();
        //}
        //else if(alphaJudge < 3.0f)
        //{
        //    StartAlpha();
        //}
	}
    public void AlphaOff()
    {
        var color = sr.color;
        // 新しいα値を設定
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        foreach (var icon in elementList)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        }
        Debug.Log("kenti2");

    }
    public void AlphaOn()
    {
        ModeInit();
        while (alpha <= 0.0f)
        {
            alpha -= speed;
            var color = sr.color;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            foreach (var icon in elementList)
            {
                icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
            }
        }
        Debug.Log("kenti");
    }

    void AlphaSetup()
    {
        alpha = 0.0f; // アルファ値の初期設定

        playerObj = GameObject.FindGameObjectWithTag("Player"); // プレイヤーの座標を取得
        playerPos = playerObj.transform.position;
        myTrans = this.gameObject.GetComponent<Transform>(); // 自分の座標を取得

        sr = spriteObj.GetComponent<SpriteRenderer>(); // 非表示にしたいSprite
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha); // Spriteの初期設定

        elementList = playerObj.GetComponent<Player>().myElement;
        foreach (var icon in elementList)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        }
    }

    void ModeInit()
    {
        playerPos = playerObj.transform.position;
        oldJudge = alphaJudge;
        alphaJudge = Mathf.Abs(myTrans.position.x - playerPos.x);
        ModeState(alphaJudge);
    }

    void ModeState(float judge)
    {
        if (judge < 3.0f && alpha <= 0)
        {
            actionMode = Mode.ALPHAOFF;
        }
        if (judge > 3.0f && alpha >= 1 && changed == true)
        {
            actionMode = Mode.ALPHAON;
        }
        if (judge > 10.0f)
        {
            actionMode = Mode.WAIT;
        }
        if (judge < 10.0f && judge > 3.0)
        {
            actionMode = Mode.MONITOR;
        }
    }

    void WaitMode()
    {
        if (Time.frameCount % 20 == 0)
        {
            ModeInit();
        }
    }

    void MonitorMode()
    {
        ModeInit();
    }

    void AlphaLock(float alpha)
    {
        ModeInit();
        if (alpha == 0)
        {
            alpha = 0;
        }
        if (alpha ==1)
        {
            alpha = 1;
        }
    }
}
