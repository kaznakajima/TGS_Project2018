using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireGimmick : GimmickController
{
    // ギミック処理
    public override void GimmickAction()
    {
        Animator gimmickAnim = GetComponent<Animator>();
        gimmickAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.FOREST.FIRE);
        Destroy(gameObject, 2.0f);
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
            // 鏡の属性が炎だったらギミック作動
            if (rayHit.collider.name == objName)
            {
                if (rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.FIRE)
                {
                    Mirror mirror = rayHit.collider.gameObject.GetComponent<Mirror>();
                    // ギミックが作動するためリセットをできなくする
                    mirror.canReset = false;
                    gimmickMaxRay = 0.0f;
                    GimmickAction();
                    // ミラーの消去コルーチン開始
                    StartCoroutine(mirror.DestroyAnimation(0.0f, 0.0f, 2.0f));
                }
                if(rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.WIND)
                {
                    gimmickMaxRay = 0.0f;
                    ForestBreak();
                    // ミラーの消去コルーチン開始
                    StartCoroutine(rayHit.collider.gameObject.GetComponent<Mirror>().DestroyAnimation(0.0f, 0.0f, 2.0f));
                }
            }
        }
    }

    // 風できるアニメーション
    void ForestBreak()
    {
        Animator gimmickAnim = GetComponent<Animator>();
        gimmickAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.FOREST.BREAK);
        GameObject childTree = GetComponentInChildren<CapsuleCollider>().gameObject;

        StartCoroutine(BreakForest(childTree));
    }

    IEnumerator BreakForest(GameObject childTree)
    {
        yield return new WaitForSeconds(2.25f);

        childTree.transform.parent = null;
        childTree.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX |  RigidbodyConstraints.FreezePositionZ;
        childTree.GetComponent<Rigidbody>().velocity = Vector2.zero;
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RayHit(-transform.right, "Enemy");
    }
}
