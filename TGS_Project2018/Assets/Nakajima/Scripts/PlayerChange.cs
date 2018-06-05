using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : PageController
{
    // ページのめくる値 (-1 ページを開いた状態、1 ページを閉じた状態)
    public float pageFlip;

    // ページのRenderer ※スクリーンショット用
    //[SerializeField]
    MeshRenderer pageRenderer;

    // ページが変更中かどうか
    bool pageChange;

    // Use this for initialization
    void Start () {
        // Rendererの取得
        pageRenderer = GetComponent<MeshRenderer>();
        pageFlip = -1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeFlag()
    {
        StartCoroutine(PageAnimation(pageFlip, 10.0f));
    }

    //ページをめくる、閉じるアニメーション
    public override IEnumerator PageAnimation(float flip, float pageSp)
    {
        // ページが開いた、閉じた状態で変更中ならbreak
        if (pageFlip < -1 && pageChange || pageFlip > 1 && pageChange)
        {
            pageChange = false;
            yield break;
        }

        yield return new WaitForSeconds(0.05f);

        pageChange = true;

        // ページを開くか、閉じるか(-1 開く、1 閉じる)
        float flipValue = flip * -1;

        // ページを開く、閉じる
        pageFlip += pageSp * flipValue * Time.deltaTime;
        // Rendererに反映
        pageRenderer.material.SetFloat("_Flip", pageFlip);

        StartCoroutine(PageAnimation(flip, pageSp));
    }

}
