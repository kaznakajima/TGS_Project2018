using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    // Rayの長さ
    public float gimmickMaxRay;

    // ギミックごとのアクション
    // 使い方(継承先)　public override void GimmickAction()
    //                         {
    //                             ここに処理を書く    
    //                         }
    public virtual void GimmickAction()
    {

    }

    // ギミックごとのRay判定
    // 使い方(継承先)　public override void RayHit(Vector3 変数名, string 変数名)
    //                         {
    //                             ここに処理を書く    
    //                         }
    public virtual void RayHit(Vector3 direction, string objName)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
