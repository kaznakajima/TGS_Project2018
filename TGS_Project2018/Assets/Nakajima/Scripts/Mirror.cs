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
    // 透明度
    [HideInInspector]
    public float mirrorAlpha;

    // Playerが映っている状態かどうか
    [HideInInspector]
    public bool isMirror;

    // プレイヤーを映すためのオブジェクト(のちのち消す)
    public GameObject mirrorObj;

    // ギミックが作動したかどうか
    [HideInInspector]
    public bool isGimmick = false;

    // 雨用のエフェクト
    [SerializeField]
    GameObject rainObj;
    [HideInInspector]
    public GameObject rainObjInstance;

    // リセット用のObject
    [SerializeField]
    GameObject resetTree;
    [SerializeField]
    GameObject resetMirror;

    // 自身のAudioSource
    [HideInInspector]
    public AudioSource myAudio;

    // ギミックのSE
    [SerializeField]
    AudioClip[] SE;

    // 判定時間
    float Interval;

    // Use this for initialization
    void Start () {
        // 方向を決定
        direction = -transform.right;
        // 自身のAudioSourceを取得
        myAudio = GetComponent<AudioSource>();
        // 映っている状態のSpriteを不可視に
        mirrorObj.GetComponent<SpriteRenderer>().color = new Color(mirrorObj.GetComponent<SpriteRenderer>().color.r, mirrorObj.GetComponent<SpriteRenderer>().color.g,
            mirrorObj.GetComponent<SpriteRenderer>().color.b, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
        // Ray判定
        RayHit(direction, "Character");
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
                isMirror = true;
                RayObjAction(rayHit.collider.gameObject);
            }
        }
    }

    // Rayにヒットしたオブジェクトごとの処理
    void RayObjAction(GameObject gameObj)
    {
        // ステータスをチェックし、変化できないなら return
        Player player = gameObj.GetComponent<Player>();
        if (status != STATUS.NONE || status == player.status
            || player.changeFlg)
        {
            return;
        }

        if(player.status == STATUS.TRADE)
        {
            SingletonMonoBehaviour<FlashController>.Instance.Flash(player, this);
            maxRay = 0.0f;
            return;
        }

        // 姿を変える
        FormChangeBefore(player);
    }

    public void PositionChange(Player player)
    {
        // playerの位置を保存し、playerの位置を変更
        Vector3 playerPos = player.transform.position;
        Vector3 mirrorPos = transform.position;
        //player.transform.position = transform.position;

        player.transform.DOMove(new Vector3(mirrorPos.x, mirrorPos.y, 0.0f), 0.01f).OnComplete(() =>
        {
            player.status = STATUS.NONE;
        });

        transform.DOMove(new Vector3(playerPos.x, playerPos.y, 0.0f), 0.01f).OnComplete(() =>
        {
            // 自身の位置を変更
            //transform.position = playerPos;
            playerPos.z = -1.0f;
            mirrorObj.transform.position = playerPos;
            direction *= -1;
            maxRay = 3.0f;

            if (player.transform.position.x > transform.position.x)
            {
                StatusChenge(STATUS.TRADE);
            }
            else
            {
                StatusChenge(STATUS.NONE);
            }
        });
    }

    // 姿を変える準備
    void FormChangeBefore(Player player)
    {
        isMirror = false;

        GameObject Destroymirror = mirrorObj;
        Destroymirror.SetActive(false);

        maxRay = 0.0f;

        // 歪みシェーダーへ変更
        statusSr.material.shader = statusMaterial[1].shader;

        if ((int)status == 0)
        {
            myAudio.PlayOneShot(SE[(int)status]);
        }

        // 処理が終わったら姿を変える
        transform.DOScale(new Vector3(0.0f, 0.0f, 1.0f), 1.0f).OnComplete(() =>
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

        // 処理が終わったらシェーダー切り替え
        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f).OnComplete(() =>
        {
            // 通常のシェーダーへ
            statusSr.material.shader = statusMaterial[0].shader;

            // ステータス更新
            status = playerSt;

            maxRay = 3.0f;

            // 雨だったらエフェクト生成
            if (status == STATUS.WATER)
            {
                if(rainObjInstance == null)
                {
                    rainObjInstance = Instantiate(rainObj, transform);
                    rainObj.GetComponent<ParticleSystem>().Stop();
                    myAudio.PlayOneShot(SE[(int)status]);
                    ResetController.resetIsonFlg = true;
                }
            }
            else
            {
                myAudio.PlayOneShot(SE[(int)status]);
                ResetController.resetIsonFlg = true;
            }

            DOTween.To(() => Interval, time =>
             Interval = time, 0.25f, 1.5f).OnComplete(() =>
             {
                 if (isGimmick == false)
                 {
                     SingletonMonoBehaviour<ResetController>.Instance.ResetExcution();
                 }
             });
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
            case STATUS.TRADE:
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.MIRROR.IDLE_RIGHT);
                break;
        }
    }

    public IEnumerator DestroyAnimation(float x, float y, float time)
    {
        // レイヤーの変更
        //gameObject.layer = 9;

        yield return new WaitForSeconds(0.5f);

        DOTween.To(() => myAudio.volume, volume => myAudio.volume = volume, 0.0f, time);

        // 処理が終わったら姿を変える
        transform.DOScale(new Vector3(x, y, 1.0f), time).OnComplete(() =>
        {
            SingletonMonoBehaviour<ResetController>.Instance.canReset = true;

            Destroy(gameObject);
        });
    }
}
