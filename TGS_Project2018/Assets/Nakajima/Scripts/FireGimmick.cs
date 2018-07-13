using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireGimmick : GimmickController
{
    // 時間経過
    float Interval = 0.0f;

    // リセット可能判定
    bool resetFlg = false;

    // ギミック処理
    public override void GimmickAction()
    {
        Interval = 0.0f;
        // 燃えるアニメーション再生
        Animator gimmickAnim = GetComponent<Animator>();
        gimmickAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.FOREST.FIRE);

        // ある程度時間がたったら削除
        DOTween.To(() => Interval, time => Interval = time, 1.0f, 2.0f).OnComplete(() =>
         {
             Destroy(gameObject);
         });
        
    }

    // Ray判定
    public override void RayHit(Vector3 direction, string objName)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, gimmickMaxRay))
        {
            // 鏡の属性が炎だったらギミック作動
            if (rayHit.collider.name == objName)
            {
                Mirror mirror = rayHit.collider.gameObject.GetComponent<Mirror>();
                if (rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.FIRE)
                {
                    mirror.isGimmick = true;
                    ResetController.resetIsonFlg = false;
                    // ゴールだったらリセットが必要
                    if (gameObject.transform.parent.name == "GoalForest(Clone)")
                    {
                        SingletonMonoBehaviour<ResetController>.Instance.TreePos = gameObject.transform.parent.position;
                        mirror.isGimmick = false;
                    }
                    // ゴールじゃなかったらリセットしなくていい
                    else
                    {
                        resetFlg = false;
                    }
                    gimmickMaxRay = 0.0f;
                    GimmickAction();
                    // ミラーの消去コルーチン開始
                    StartCoroutine(mirror.DestroyAnimation(0.0f, 0.0f, 2.0f));
                }
                else if(rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.WIND)
                {
                    ResetController.resetIsonFlg = false;
                    mirror.isGimmick = false;
                    // ゴールだったらリセットしなくていい
                    if (gameObject.transform.parent.name == "GoalForest(Clone)")
                    {
                        resetFlg = false;
                        mirror.isGimmick = true;
                    }
                    gimmickMaxRay = 0.0f;
                    ForestBreak();
                    // ミラーの消去コルーチン開始
                    StartCoroutine(mirror.DestroyAnimation(0.0f, 0.0f, 3.0f));
                }else if (rayHit.collider.gameObject.GetComponent<Mirror>().status != StatusController.STATUS.FIRE &&
                    rayHit.collider.gameObject.GetComponent<Mirror>().status != StatusController.STATUS.WIND &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status != StatusController.STATUS.NONE)
                {
                    StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                    StartCoroutine(SingletonMonoBehaviour<PageChange>.Instance.ScreenShot());
                }
            }
        }
    }

    // 風できるアニメーション
    void ForestBreak()
    {
        // 木を切るアニメーション再生
        Animator gimmickAnim = GetComponent<Animator>();
        gimmickAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.FOREST.BREAK);
        GameObject childTree = GetComponentInChildren<CapsuleCollider>().gameObject;

        StartCoroutine(BreakAnimation(childTree));
    }

    IEnumerator BreakAnimation(GameObject childTree)
    {
        yield return new WaitForSeconds(2.25f);

        // 木の幹だけを残す
        childTree.transform.parent = null;
        childTree.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX |  RigidbodyConstraints.FreezePositionZ 
            | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        childTree.GetComponent<Rigidbody>().velocity = Vector2.zero;

        childTree.GetComponent<Goal>().isBreak = true;
        // リセットが必要ならリセットアイコン表示
        if (resetFlg)
        {
            ResetController.resetIsonFlg = true;
        }
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        resetFlg = true;
    }
	
	// Update is called once per frame
	void Update () {
        RayHit(-transform.right, "Enemy");
    }
}
