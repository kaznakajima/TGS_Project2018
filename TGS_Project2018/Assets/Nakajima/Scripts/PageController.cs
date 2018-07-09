using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : SingletonMonoBehaviour<PageChange>
{
    // ページをめくるアニメーション
    public virtual IEnumerator PageAnimation(float flip, float pageSp)
    {
        yield return null;
    }

    // スクリーンショット用
    public virtual IEnumerator ScreenShot()
    {
        yield return null;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
