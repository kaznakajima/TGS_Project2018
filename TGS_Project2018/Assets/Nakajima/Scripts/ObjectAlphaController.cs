using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectAlphaController : MonoBehaviour
{
    // Mapの参照
    public GameObject map;

    // Objectのアルファ値
    float objAlpha = 0.0f;

    // Playerの参照
    public Player[] GetPlayer()
    {
        return FindObjectsOfType<Player>().ToArray(); 
    }

    // Mirrorの参照
    public Mirror[] GetMirror()
    {
        return map.GetComponentsInChildren<Mirror>().ToArray();
    }

	// Use this for initialization
	void Start () {
        foreach (var player in GetPlayer())
        {
            foreach (var playerSr in player.myElement)
            {
                playerSr.color = new Color(playerSr.color.r, playerSr.color.g, playerSr.color.b, objAlpha);
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
	}

    // 距離を見てアルファの調整
    void DistanceCheck()
    {
        foreach(var player in GetPlayer())
        {
          if(player.changeFlg || (int)player.status != 0)
            {
                AlphaChange(0.0f);
                return;
            }
        }

        if(GetPlayer().Min(playerPos => GetMirror().Min(mirrorPos => 
            Mathf.Abs(playerPos.transform.position.x - mirrorPos.transform.position.x))) <= 3)
        {
            AlphaChange(0.75f);
        }
        else if(GetPlayer().Min(playerPos => GetMirror().Min(mirrorPos =>
            Mathf.Abs(playerPos.transform.position.x - mirrorPos.transform.position.x))) > 3)
        {
            AlphaChange(0.0f);
        }
    }

    // 実際にアルファの値の変更
    void AlphaChange(float nextAlpha)
    {
        // アルファの値が同じなら早期リターン
        if (objAlpha == 0.0f && nextAlpha == 0.0f ||
            objAlpha == 0.75f && nextAlpha == 0.75f)
        {
            Debug.Log("とおった");
            return;
        }

        // 次のアルファの値に設定
        objAlpha = Mathf.Lerp(objAlpha, nextAlpha, 10.0f * Time.deltaTime);

        foreach(var player in GetPlayer())
        {
            foreach (var playerSr in player.myElement)
            {
                playerSr.color = new Color(playerSr.color.r, playerSr.color.g, playerSr.color.b, objAlpha);
            }
        }
        foreach(var mirror in GetMirror())
        {
            mirror.mirrorObj.GetComponent<SpriteRenderer>().color = new Color(mirror.mirrorObj.GetComponent<SpriteRenderer>().color.r, 
                mirror.mirrorObj.GetComponent<SpriteRenderer>().color.g, mirror.mirrorObj.GetComponent<SpriteRenderer>().color.b, objAlpha);
        }

        
    }
}
