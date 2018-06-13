using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
    // スクロールの大きさ
    [SerializeField]
    public float scrollSpeedX = 0.1f;

    void Start()
    {
        // SpriteRendererを取得
        GetComponent<SpriteRenderer>().sharedMaterial.SetTextureOffset("_MainTex", Vector2.zero);
    }
    void Update()
    {
        if(scrollSpeedX == 0)
        {
            return;
        }

        // 背景のスクロール
        float x = Mathf.Repeat(Time.time * scrollSpeedX, 1);
        Vector2 offset = new Vector2(x, 0);
        GetComponent<SpriteRenderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
