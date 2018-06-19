using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class ObjectAlphaController : MonoBehaviour
{
    // Mapの参照
    public GameObject map;

    // Objectのアルファ値
    float objAlpha = 0.0f;
    // プレイヤーアイコンのアルファ値
    float playerIconAlpha = 0.0f;
    // アルファ値比較用変数
    float comparison = 0.0f;

    // アルファ判定用
    bool isReturn;

    // Playerを取得
    public Player[] GetPlayer()
    {
        return FindObjectsOfType<Player>().ToArray(); 
    }

    // Mirrorを取得
    public Mirror[] GetMirror()
    {
        return map.GetComponentsInChildren<Mirror>().ToArray();
    }

	// Use this for initialization
	void Start () {
        // アイコンを隠す
        foreach (var player in GetPlayer())
        {
            foreach (var playerSr in player.myElement)
            {
                playerSr.color = new Color(playerSr.color.r, playerSr.color.g, playerSr.color.b, objAlpha);
            }
            foreach(var Icons in player.playerIcons)
            {
                Icons.color = new Color(Icons.color.r, Icons.color.g, Icons.color.b, playerIconAlpha);
            }
        }
        foreach (var mirror in GetMirror())
        {
            mirror.mirrorObj.GetComponent<SpriteRenderer>().color = new Color(mirror.mirrorObj.GetComponent<SpriteRenderer>().color.r,
                mirror.mirrorObj.GetComponent<SpriteRenderer>().color.g, mirror.mirrorObj.GetComponent<SpriteRenderer>().color.b, objAlpha);
        }
    }
	
	// Update is called once per frame
	void Update () {
        DistanceCheck();
        IconsShow();
    }

    // 距離を見てアルファの調整
    void DistanceCheck()
    {
        // 鏡がいなければリターン
        if(GetMirror().Length == 0)
        {
            return;
        }

        // プレイヤーの状態をチェック
        foreach (var player in GetPlayer())
        {
            // プレイヤーが変化中はアイコンを出さない
            if (player.changeFlg || (int)player.status != 0)
            {
                AlphaChange(0.0f);
                return;
            }
        }

        // プレイヤーと鏡が近づいたらアイコン表示
        if(GetPlayer().Min(playerPos => GetMirror().Min(mirrorPos => 
            Mathf.Abs(playerPos.transform.position.x - mirrorPos.transform.position.x))) <= 3)
        {
            if(GetMirror().Min(mirrorStatus => (int)mirrorStatus.status == 0))
            {
                AlphaChange(0.75f);
            }
        }
        // 離れたらアイコンを隠す
        else if(GetPlayer().Min(playerPos => GetMirror().Min(mirrorPos =>
            Mathf.Abs(playerPos.transform.position.x - mirrorPos.transform.position.x))) > 3)
        {
            AlphaChange(0.0f);
        }
    }

    // プレイヤーアイコン表示、非表示用メソッド
    void IconsShow()
    {
        // プレイヤーの状態を確認
        foreach (var playerStatus in GetPlayer())
        {
            // 通常状態なら非表示
            if((int)playerStatus.status == 0)
            {
                comparison = 0.0f;
                DOTween.To(() => playerIconAlpha, alpha => playerIconAlpha = alpha, comparison, 1.0f);
                //playerIconAlpha = Mathf.Lerp(playerIconAlpha, comparison, 10.0f * Time.deltaTime);
                foreach (var Icons in playerStatus.playerIcons)
                {
                    // 見えているやつだけ透過
                    if(Icons.color.a > 0.0f)
                    {
                        Icons.color = new Color(Icons.color.r, Icons.color.g, Icons.color.b, playerIconAlpha);
                    }
                }
            }
            // 属性状態ならアイコン表示
            else
            {
                // 一度リセット
                foreach (var Icons in playerStatus.playerIcons)
                {
                    Icons.color = new Color(Icons.color.r, Icons.color.g, Icons.color.b, 0.0f);
                }

                comparison = 0.75f;
                DOTween.To(() => playerIconAlpha, alpha => playerIconAlpha = alpha, comparison, 1.0f);
                //playerIconAlpha = Mathf.Lerp(playerIconAlpha, 0.75f, 10.0f * Time.deltaTime);
                playerStatus.playerIcons[(int)playerStatus.status - 1].color = new Color(playerStatus.playerIcons[(int)playerStatus.status - 1].color.r,
                    playerStatus.playerIcons[(int)playerStatus.status - 1].color.g, playerStatus.playerIcons[(int)playerStatus.status - 1].color.b, playerIconAlpha);
            }
        }
    }

    // 実際にアルファの値の変更
    void AlphaChange(float nextAlpha)
    {

        // アルファの値が同じなら早期リターン
        if (objAlpha == 0.0f && nextAlpha == 0.0f ||
            objAlpha == 0.75f && nextAlpha == 0.75f)
        {
            return;
        }

        // 次のアルファの値に設定
        DOTween.To(() => objAlpha, alpha => objAlpha = alpha, nextAlpha, 1.0f);
        //objAlpha = Mathf.Lerp(objAlpha, nextAlpha, 10.0f * Time.deltaTime);

        foreach (var mirror in GetMirror())
        {
            if (mirror.mirrorObj == null)
            {
                return;
            }

            mirror.mirrorObj.GetComponent<SpriteRenderer>().color = new Color(mirror.mirrorObj.GetComponent<SpriteRenderer>().color.r,
                mirror.mirrorObj.GetComponent<SpriteRenderer>().color.g, mirror.mirrorObj.GetComponent<SpriteRenderer>().color.b, objAlpha);

            foreach (var player in GetPlayer())
            {
                
                foreach (var playerSr in player.myElement)
                {
                    playerSr.color = new Color(playerSr.color.r, playerSr.color.g, playerSr.color.b, objAlpha);
                }
            }
        }
    }
}
