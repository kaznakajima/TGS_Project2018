using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FlashController : SingletonMonoBehaviour<FlashController>
{
    // フラッシュ用のImage
    [HideInInspector]
    public SpriteRenderer flashImage;

    // スケール値
    [HideInInspector]
    public float alpha;

	// Use this for initialization
	void Start () {
        alpha = 0.0f;
        flashImage = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, alpha);
    }

    // 閃光メソッド
    public void Flash(Player player, Mirror mirror)
    {
        DOTween.To(() => alpha, value => alpha = value, 1.0f, 1.5f).OnComplete(() =>
        {
            mirror.PositionChange(player);
            DOTween.To(() => alpha, value => alpha = value, 0.0f, 1.5f);
        });
    }
}
