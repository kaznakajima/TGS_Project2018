using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceGimmick : GimmickController
{
    [SerializeField]
    GameObject steam;

    // 滑る向き
    public static float moveX = 3.0f;

    // 滑る判定
    [HideInInspector]
    public bool isSlope = true;

    // ギミック処理
    public override void GimmickAction()
    {
        GameObject obj = Instantiate(steam) as GameObject;
        obj.transform.position = transform.position;
        // レイヤー変更
        //gameObject.layer = 9;

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
        Ray rightRay = new Ray(transform.position + new Vector3(1.0f, 0.0f, 0.0f), direction);
        Ray leftRay = new Ray(transform.position + new Vector3(-1.0f, 0.0f, 0.0f), direction);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, gimmickMaxRay) || Physics.Raycast(rightRay, out rayHit, gimmickMaxRay) ||
            Physics.Raycast(leftRay, out rayHit, gimmickMaxRay))
        {
            // 鏡がのっているなら滑らない
            if (rayHit.collider.name == objName)
            {
                isSlope = false;
            }

            // 鏡の属性が炎だったらギミック作動
            if (rayHit.collider.name == objName &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.FIRE)
            {
                Mirror mirror = rayHit.collider.gameObject.GetComponent<Mirror>();
                mirror.isGimmick = true;
                gimmickMaxRay = 0.0f;
                GimmickAction();
                ResetController.resetIsonFlg = false;
                // ミラーの消去コルーチン開始
                StartCoroutine(mirror.DestroyAnimation(0.0f, 0.0f, 2.0f));
            } else if (rayHit.collider.name == objName &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status != StatusController.STATUS.FIRE &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status != StatusController.STATUS.NONE)
            {
                StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                StartCoroutine(SingletonMonoBehaviour<PageChange>.Instance.ScreenShot());
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

    void OnCollisionStay(Collision c)
    {
        if (c.gameObject.name == "Character" && isSlope)
        {
            Player player = c.gameObject.GetComponent<Player>();
            player.isSlope = true;
            if (player.statusAnim.GetInteger("BluckAnim") == 0 || player.statusAnim.GetInteger("BluckAnim") == 1)
            {
                moveX = 3.0f;
            }
            else if (player.statusAnim.GetInteger("BluckAnim") == 9 || player.statusAnim.GetInteger("BluckAnim") == 2)
            {
                moveX = -3.0f;
            }
        }
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
}
