using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceGimmick : GimmickController
{
    [SerializeField]
    GameObject steam;

    // 滑る向き
    public static float moveX = 5.0f;

    // 滑る判定
    [HideInInspector]
    public bool isSlope = true;
    // true 右　false 左
    [HideInInspector]
    public static bool direction = false;

    // ギミック処理
    public override void GimmickAction()
    {
        GameObject obj = Instantiate(steam) as GameObject;
        obj.transform.position = transform.position;
        // レイヤー変更
        gameObject.layer = 9;

        transform.DOScale(Vector3.zero, 3.0f).OnComplete(() =>
        {
            // Particleを取得し、ループをはずす
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();
            particle.loop = false;

            Destroy(gameObject);
        });
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
            if (rayHit.collider.name == objName)
            {
                isSlope = false;
            }

            // 鏡の属性が炎だったらギミック作動
            if (rayHit.collider.name == objName &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.FIRE)
            {
                Mirror mirror = rayHit.collider.gameObject.GetComponent<Mirror>();
                gimmickMaxRay = 0.0f;
                GimmickAction();
                ResetController.resetIsonFlg = false;
                // ミラーの消去コルーチン開始
                StartCoroutine(mirror.DestroyAnimation(0.0f, 0.0f, 2.0f));
            }
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        RayHit(transform.up, "Enemy");
    }

    void OnCollisionEnter(Collision c)
    {
        
    }
    void OnCollisionStay(Collision c)
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "Ivy")
        {
            RainGimmick rain = c.gameObject.GetComponent<RainGimmick>();
            rain.isHit = true;
            rain.GimmickAction();
        }
    }

    void OnCollisionExit(Collision c)
    {

    }
}
