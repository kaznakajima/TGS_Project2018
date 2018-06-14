using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PageChange : PageController
{ 
    // ページのめくる値 (-1 ページを開いた状態、1 ページを閉じた状態)
    public float pageFlip;
    
    // ページのRenderer ※スクリーンショット用
    MeshRenderer pageRenderer;

    // ページが変更中かどうか
    public bool pageChange;

    // シーンが変わるかどうか
    public bool SceneChange;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        // Rendererの取得
        pageRenderer = GetComponent<MeshRenderer>();

        pageRenderer.material.SetTexture("_MainTex", SingletonMonoBehaviour<ScreenShot>.Instance.tex2D);

        // スクリーンショットを最前面へ
        pageFlip = 1;
        pageRenderer.material.SetFloat("_Flip", pageFlip);

        StartCoroutine(PageAnimation(pageFlip, 5.0f));
    }
	
	// Update is called once per frame
	void Update () {

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

        // ページをめくる処理中
        pageChange = true;

        // ページを開くか、閉じるか(-1 開く、1 閉じる)
        float flipValue = flip * -1;

        // ページを開く、閉じる
        pageFlip += pageSp * flipValue * Time.deltaTime;
        // Rendererに反映
        pageRenderer.material.SetFloat("_Flip", pageFlip);

        StartCoroutine(PageAnimation(flip, pageSp));
    }

    // 現在のスクリーンショットを撮る
    public override IEnumerator ScreenShot()
    {
        yield return new WaitForEndOfFrame();

        // 現在の状態をスクリーンショットにする
        var texture = new Texture2D(Screen.width, Screen.height);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Rendererに反映
        pageRenderer.material.SetTexture("_MainTex", texture);

        // スクリーンショットを最前面へ
        pageFlip = 1;
        pageRenderer.material.SetFloat("_Flip", pageFlip);

        // シーン変更するならCanvasを不可視状態へ
        if (SceneChange)
        { 
            
        }

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(PageAnimation(pageFlip, 5.0f));
    }
}
