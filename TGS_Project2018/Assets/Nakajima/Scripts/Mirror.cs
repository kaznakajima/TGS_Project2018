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
    float mirrorAlpha;

    // プレイヤーを映すためのオブジェクト(のちのち消す)
    public GameObject mirrorObj;

    // ギミックが作動したかどうか
    [HideInInspector]
    public bool isGimmick;

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

    // Use this for initialization
    void Start () {
        // 方向を決定
        direction = -transform.right;
        // 自身のAudioSourceを取得
        myAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        // Ray判定
        RayHit(direction, "Character");

        if(!isGimmick && (int)status != 0)
        {
            ResetController.resetIsonFlg = true;
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
                RayObjAction(rayHit.collider.gameObject);
            }
        }
    }

    // Rayにヒットしたオブジェクトごとの処理
    void RayObjAction(GameObject gameObj)
    {
        // ステータスをチェックし、変化できないなら return
        Player player = gameObj.GetComponent<Player>();
        if (status != STATUS.NONE || status == player.status)
        {
            return;
        }

        // 姿を変える
        FormChangeBefore(player);
    }

    // 姿を変える準備
    void FormChangeBefore(Player player)
    {

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
                }
                return;
            }
            myAudio.PlayOneShot(SE[(int)status]);
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

    public IEnumerator DestroyAnimation(float x, float y, float time)
    {
        yield return new WaitForSeconds(0.5f);

        DOTween.To(() => myAudio.volume, volume => myAudio.volume = volume, 0.0f, time);

        // 処理が終わったら姿を変える
        transform.DOScale(new Vector3(x, y, 1.0f), time).OnComplete(() =>
        {
            // リセット可能にする
            isGimmick = false;
            SingletonMonoBehaviour<ResetController>.Instance.canReset = true;
            Destroy(gameObject);
        });
    }
}
