using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Mirror : StatusController
{

    // rayの方向
    Vector3 direction;
    // rayの長さ
    [SerializeField]
    float maxRay;
    // AudioSource
    [SerializeField]
    AudioSource mirrorAudio;
    // AudioClipの配列
    [SerializeField]
    AudioClip[] mirrorSE;

    // 歪みの強さ
    [SerializeField, Range(150, 500)]
    float distortionX;
    [SerializeField, Range(150, 500)]
    float distortionY;
    [SerializeField, Range(150, 500)]
    float distortionZ;

    // Use this for initialization
    void Start () {
        // 方向を決定
        direction = -transform.right;
	}
	
	// Update is called once per frame
	void Update () {

        if (CameraRangeCheck(transform.position) != true)
        {
            //MirrorReset(mirrorTex, mirrorSp);
        }

        // Ray判定
        RayHit(direction, "Character");
    }

    // カメラ範囲かどうか
    bool CameraRangeCheck(Vector3 _pos)
    {
        // カメラの範囲検出
        Vector3 viewPos = Camera.main.WorldToViewportPoint(_pos);
        // カメラ範囲外かチェック
        if(viewPos.x < -0.1f || viewPos.x > 1.1f ||
       viewPos.y < -0.1f || viewPos.y > 1.0f)
        {
            // 範囲外
            return false;
        }
        else
        {
            // 範囲内
            return true;
        }
    }

    // Rayの判定
    void RayHit(Vector3 direction, string objName)
    {
        // Rayを飛ばす
        Ray ray = new Ray(transform.position, direction);
        RaycastHit rayHit;
        // デバッグ用の可視化
        Debug.DrawRay(transform.position, direction * maxRay, Color.red);
        // Hitしたオブジェクトが指定したオブジェクトだったら実行
        if (Physics.Raycast(ray, out rayHit, maxRay))
        {
            if (rayHit.collider.name == objName)
            {
                maxRay = 0.0f;
                RayObjAction(rayHit.collider.gameObject);
            }
        }
    }

    // Rayにヒットしたオブジェクトごとの処理
    void RayObjAction(GameObject gameObj)
    {
        // 姿を変える
        //Move move = gameObj.GetComponent<Move>();
        Player player = gameObj.GetComponent<Player>();
        FormChangeBefore(player);
    }

    void MirrorReset(Texture tex, Sprite sp)
    {
        if(statusSr.material.GetTexture("_MainTex") != tex)
        {
            statusSr.material.SetTexture("_MainTex", tex);
            statusSr.material.shader = statusMaterial[0].shader;
            StatusChenge(STATUS.NONE);
        }
        else
        {
            return;
        }
    }

    // 姿を変える準備
    void FormChangeBefore(Player player)
    {
        // ステータスをチェックし、同じだったら return
        if (player.status == status)
        {
            maxRay = 3.0f;
            return;
        }

        // 歪みシェーダーへ変更
        statusSr.material.shader = statusMaterial[1].shader;

        // 歪みの強さ
        statusSr.material.SetFloat("_distortionX", distortionX);
        statusSr.material.SetFloat("_distortionY", distortionY);
        statusSr.material.SetFloat("_distortionZ", distortionZ);

        //mirrorAudio.PlayOneShot(mirrorSE[(int)STATUS.NONE]);

        // 処理が終わったら姿を変える
        transform.DOScale(new Vector3(0.0f, 0.0f, 1.0f), 2.0f).OnComplete(() =>
        {
            FormChangeAfter(player);
        });
    }

    // 姿を変える
    void FormChangeAfter(Player player)
    {

        // Playerのステータスを取得
        STATUS playerSt = player.GetComponent<Player>().status;
        StatusChenge(playerSt);

        //mirrorAudio.PlayOneShot(mirrorSE[(int)STATUS.NONE]);

        // 処理が終わったらシェーダー切り替え
        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 2.0f).OnComplete(() =>
        {
            // 通常のシェーダーへ
            statusSr.material.shader = statusMaterial[0].shader;

            // ステータス更新
            status = playerSt;

            //mirrorAudio.PlayOneShot(mirrorSE[(int)status]);

            maxRay = 3.0f;
        });
    }

    // ステータスを変化
    public override void StatusChenge(STATUS _status)
    {
        // ステータスによって変化する形を指定
        switch (_status)
        {
            // 通常
            case STATUS.NONE:
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.MIRROR.IDLE);
                break;
            // 炎
            case STATUS.FIRE:
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.MIRROR.FIRE);
                break;
            // 水
            case STATUS.WATER:
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.MIRROR.WATER);
                break;
            // 風
            case STATUS.WIND:
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.MIRROR.WIND);
                break;
            // 地
            case STATUS.EARTH:
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.MIRROR.STONE);
                break;
        }
    }

    public IEnumerator DestroyAnimation(float x, float y)
    {
        yield return new WaitForSeconds(1.0f);

        // 処理が終わったら姿を変える
        transform.DOScale(new Vector3(x, y, 1.0f), 3.0f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
