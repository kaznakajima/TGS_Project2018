using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RainGimmick : GimmickController
{
    // Mirrorの判定用
    bool isMirror;
    GameObject mirrorObj;

    // 当たり判定
    [HideInInspector]
    public bool isHit;

    // 自身のAudioSource
    AudioSource myAudio;

    // ギミック処理
    public override void GimmickAction()
    {
        float nextPosition;
        // 何かに当たったらその時点でストップ
        if (isHit)
        {
            SingletonMonoBehaviour<ResetController>.Instance.IvyObj = gameObject;
            nextPosition = transform.position.y;
        }
        // 当たっていないなら伸び続ける
        else
        {
            myAudio.PlayOneShot(myAudio.clip);
            nextPosition = transform.position.y + 10.18f;
            SingletonMonoBehaviour<ResetController>.Instance.IvyPos = transform.position;
        }

        // 何かに当たっていたらリセット表示
        transform.DOMove(new Vector3(transform.position.x, nextPosition, transform.position.z), 2.0f).OnComplete(() =>
        {
            if (isHit)
            {
                ResetController.resetIsonFlg = true;
                SingletonMonoBehaviour<ResetController>.Instance.IvyObj = null;
                StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                StartCoroutine(SingletonMonoBehaviour<PageChange>.Instance.ScreenShot());
            }
        });

        isMirror = false;
    }

    // Ray判定
    public override void RayHit(Vector3 direction, string objName)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, gimmickMaxRay))
        {
            // 鏡の属性が水だったらギミック作動
            if (rayHit.collider.name == objName &&
                rayHit.collider.gameObject.GetComponent<Mirror>().status == StatusController.STATUS.WATER)
            {
                Mirror mirror = rayHit.collider.gameObject.GetComponent<Mirror>();
                mirror.isGimmick = true;
                isMirror = true;
                mirrorObj = rayHit.collider.gameObject;
                ResetController.resetIsonFlg = false;
                gimmickMaxRay = 0.0f;
                // 重力を無視する
                rayHit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                // ミラーの消去コルーチン開始
                StartCoroutine(mirror.DestroyAnimation(0.0f, 1.0f, 2.0f));
            }
        }
    }

    // Use this for initialization
    void Start () {
        myAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        RayHit(transform.up, "Enemy");

        if (isMirror && mirrorObj == null)
        {
            GimmickAction();
            gimmickMaxRay = 2.0f;
        }
    }
}
