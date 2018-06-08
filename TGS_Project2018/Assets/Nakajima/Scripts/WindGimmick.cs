using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGimmick : GimmickController
{

    // ギミック処理
    public override void GimmickAction()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.right * 300.0f);
    }

    // Ray判定
    public override void RayHit(Vector3 direction, string objName)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit rayHit;
        // デバッグ用の可視化
        Debug.DrawRay(transform.position, direction * gimmickMaxRay, Color.red);
        if (Physics.Raycast(ray, out rayHit, gimmickMaxRay))
        {
            // 鏡の属性が風だったらギミック作動
            if (rayHit.collider.name == objName &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.WIND)
            {
                gimmickMaxRay = 0.0f;
                GimmickAction();
                // ミラーの消去コルーチン開始
                StartCoroutine(rayHit.collider.gameObject.GetComponent<Mirror>().DestroyAnimation(0.0f, 1.0f));
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RayHit(-transform.right, "Enemy");
	}
}
