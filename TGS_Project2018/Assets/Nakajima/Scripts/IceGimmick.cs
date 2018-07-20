using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class IceGimmick : GimmickController
{
    // 動かす判定
    bool isActive;

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
        ChecckAroundPos();

        GameObject obj = Instantiate(steam) as GameObject;
        obj.transform.position = transform.position;
        // レイヤー変更
        //gameObject.layer = 9;

        transform.DOScale(Vector3.zero, 3.0f).OnComplete(() =>
        {
            // Particleを取得し、ループをはずす
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();

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
            }
        }
    }

    void ChecckAroundPos()
    {
        // 左右1マス分の座標
        var aroundPos = new Coordinate[]
        {
            new Coordinate((int)transform.position.x + 1, (int)transform.position.y),
            new Coordinate((int)transform.position.x - 1, (int)transform.position.y)
        };

        // 左右1マスをチェックして氷オブジェクトか判断
        foreach(var iceAround in aroundPos)
        {
            // 氷オブジェクトの取得
            var iceGimmick = GetIceGimmick(iceAround.x, iceAround.y);
            // 取得出来たら子にする
            if(iceGimmick != null)
            {
                iceGimmick.gameObject.transform.parent = transform;
            }
        }
    }

    // 氷ギミックを取得
    IceGimmick GetIceGimmick(int x, int y)
    {
        return FindObjectsOfType<IceGimmick>().FirstOrDefault(ice => (int)ice.transform.position.x == x && (int)ice.transform.position.y == y);
    }

    // Use this for initialization
    void Start() {
        isActive = false;
    }

    // Update is called once per frame
    void Update() {
        if(isActive == false)
        {
            return;
        }

        RayHit(transform.up, "Enemy");
    }

    void OnCollisionStay(Collision c)
    {
        // 何かが触れているなら処理実行
        if (c.gameObject.name == "Enemy" || c.gameObject.name == "Character")
            isActive = true;

        if (c.gameObject.name == "Character")
        {
            Player player = c.gameObject.GetComponent<Player>();
            player.rayPoint = 0.0f;
            if (isSlope)
            {
                player.isSlope = true;
                if (player.statusAnim.GetInteger("BluckAnim") == 0 || player.statusAnim.GetInteger("BluckAnim") == 2)
                {
                    moveX = 3.0f;
                }
                else if (player.statusAnim.GetInteger("BluckAnim") == 9 || player.statusAnim.GetInteger("BluckAnim") == 3)
                {
                    moveX = -3.0f;
                }
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

    void OnCollisionExit()
    {
        // 何も触れていないなら動かない
        isActive = false;
    }
}

/// <summary>
/// 座標クラス
/// </summary>
public class Coordinate
{
    public readonly int x;
    public readonly int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

