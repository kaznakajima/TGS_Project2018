using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RainGimmick : GimmickController
{
    // ギミック処理
    public override void GimmickAction()
    {
        transform.DOMove(new Vector3(transform.position.x, 3.0f, transform.position.z), 3.0f);
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
            // 鏡の属性が水だったらギミック作動
            if (rayHit.collider.name == objName &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.WATER)
            {
                gimmickMaxRay = 0.0f;
                GimmickAction();
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RayHit(transform.up, "Enemy");
	}
}
