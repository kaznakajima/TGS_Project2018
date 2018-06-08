﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RainGimmick : GimmickController
{
    // 雨用のパーティクル
    [SerializeField]
    GameObject rainObj;

    // ギミック処理
    public override void GimmickAction()
    {
        rainObj.GetComponent<ParticleSystem>().Stop();
        transform.DOMove(new Vector3(transform.position.x, -0.5f, transform.position.z), 3.0f);
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
                Instantiate(rainObj, rayHit.collider.gameObject.transform);
                GimmickAction();
                // ミラーの消去コルーチン開始
                StartCoroutine(rayHit.collider.gameObject.GetComponent<Mirror>().DestroyAnimation(0.0f, 0.0f));
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
