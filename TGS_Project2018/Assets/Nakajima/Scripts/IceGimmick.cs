using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceGimmick : GimmickController
{
    [SerializeField]
    GameObject steam;

    // 接触しているか
    bool isContact;
    // 滑る向き
    float moveX;

    // 滑らせるためのオブジェクト
    Rigidbody playerRig;
    Vector3 playerVec;

    // ギミック処理
    public override void GimmickAction()
    {
        GameObject obj = Instantiate(steam) as GameObject;
        obj.transform.position = transform.position;

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
            // 鏡の属性が炎だったらギミック作動
            if (rayHit.collider.name == objName &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.FIRE)
            {
                Mirror mirror = rayHit.collider.gameObject.GetComponent<Mirror>();
                // ギミックが作動するためリセットをできなくする
                mirror.canReset = false;
                gimmickMaxRay = 0.0f;
                GimmickAction();
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

    void SlideMove(float moveX)
    {

    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.name == "Character")
        {
            if(c.gameObject.GetComponent<Player>().statusAnim.GetInteger("BluckAnim") == 1)
            {
                isContact = true;
                moveX = 1.0f;
            }
            else if(c.gameObject.GetComponent<Player>().statusAnim.GetInteger("BluckAnim") == -1)
            {
                isContact = true;
                moveX = -1.0f;
            }
        }
    }

    void OnCollisionExit(Collision c)
    {
        if(c.gameObject.name == "Character")
        {
            isContact = false;
        }
    }
}
