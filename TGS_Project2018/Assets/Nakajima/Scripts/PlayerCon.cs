using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCon : StatusController
{

    /*
     【キャラクター移動クラス】
     操作方法 
     横移動･･･ADキーor左右矢印キー
     ジャンプ･･･Cキー
     属性変換･･･数字キー（1火属性、2水属性、3風属性、4土属性）
     ツタ上り･･･ツタの近くでQキー長押し
    */
    // privateでもインスペクター上で編集できるようにSerializeField属性を付けた

    PageChange pageChange; // ページ遷移クラス
    GameMaster gm; // ゲームマスタークラス
    UVScroll[] uvScroll = new UVScroll[2];

    [SerializeField] float playerSpeed = 1.0f; // キャラクターのスピード
    [SerializeField] float playerMaxSpeed = 1.5f; // プレイヤーの最大スピード
    [SerializeField] float playerMinSpeed = -1.5f; // プレイヤーの最小スピード
    [SerializeField] float jumpSpeed = 2.0f; // ジャンプ力
    [SerializeField] float speed; // 移動スピード
    [SerializeField] float jumpCoolDownCount = 0.0f; // ジャンプのクールダウンカウント
    [SerializeField] float jumpCoolDownLimit = 0.5f; // ジャンプ再使用までの時間
    [SerializeField] float rayRange = 1.0f; // 接地判定の距離 
    [SerializeField] float defaultRayRange; // 保存用設置判定の距離

    bool jumpFlg = false; // ジャンプ可能か
    [SerializeField] bool isGround; // 接地しているか
    [SerializeField] bool climbFlg = false; // 上下移動可能か
    [SerializeField] bool shipFlg = false; // 船に乗っているか
    [SerializeField] bool damageFlg = false; // ダメージを受けているか
    public bool changeFlg = false; // 変身しているか
    [SerializeField] bool pageChangeFlg = false; //ページがめくり終わったか
    [SerializeField] string changePageName; // ページ遷移クラスオブジェクトを取得するために必要な文字列型変数

    CharacterController myCc; // キャラクターコントローラー
    Rigidbody myRigidbody; // 自分のRigidbody
    public SpriteRenderer[] myElement = new SpriteRenderer[4];

    [SerializeField] Vector3 movePos; // 移動用変数
    [SerializeField] Vector3 endPos; // 上下移動末端の座標を格納
    [SerializeField] Vector3 climbPos; // 上下移動用ベクトル
    [SerializeField] Vector3 wayPointPos; // リスポーン地点保存用変数
    Vector3 startPos; // 上下移動開始点の座標を格納
    Vector3 oldVelocity; // なめらかに移動させるために必要な一時保存用ベクトル

    // Use this for initialization
    void Start()
    {
        statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE); // アニメーションの初期設定
        gm = GameObject.Find("Master").GetComponent<GameMaster>(); // ゲームマスターコンポーネント取得
        uvScroll = FindObjectsOfType<UVScroll>();
        foreach (var item in uvScroll)
        {
            item.scrollSpeedX = 0.0f;
        }
        pageChange = GameObject.Find(changePageName).GetComponent<PageChange>(); // ページ遷移のコンポーネント取得
        myRigidbody = this.gameObject.GetComponent<Rigidbody>(); // RigidBodyコンポーネントを取得
        defaultRayRange = rayRange;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = GroundJudgment(); // 常に接地判定を取る
        if (isGround || climbFlg) // プレイヤーが地面に設置していたら
        {
            // なめらかに移動させる
            movePos = Vector3.Lerp(oldVelocity, movePos, playerSpeed * Time.deltaTime);
            oldVelocity = movePos; // なめらかに移動させるために必要な一時保存用ベクトルを保存
            // スティックが右方向に倒れたら
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                movePos.x += speed; // 移動ベクトルにスピードを加算
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.RUN_RIGHT); // 歩行アニメーションON
                if (movePos.x >= playerMaxSpeed) // 移動ベクトルが最大スピードを超えたら
                {
                    movePos.x = playerMaxSpeed; // 移動スピードは最大スピード固定
                }
            }// スティックが左方向に倒れたら
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                // 移動ベクトルに負のスピードを加算
                movePos.x += -speed;
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.RUN_LEFT);
                if (movePos.x <= playerMinSpeed)// 移動ベクトルが最小スピードを下回ったら
                {
                    movePos.x = playerMinSpeed;// 移動スピードは最小スピード固定
                }
            }// 何も押されていなかったら
            else if (Input.GetAxisRaw("Horizontal") == 0 && changeFlg == false && status == STATUS.NONE)
            {
                movePos = new Vector3(0.0f, 0.0f, 0.0f); // 移動量は０に
                // 歩行アニメーションOFF
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE);
            }
            if (Input.GetKeyDown(KeyCode.Space) == true || Input.GetKeyDown("joystick button 6"))
            {
                StartCoroutine(pageChange.ScreenShot());
                gm.sketchBookValue -= 1; // マスタークラスの残機を減らす
            }
            if (Input.GetKeyDown(KeyCode.C) || Input.GetAxis("Vertical") <= -1.0f && jumpCoolDownCount >= jumpCoolDownLimit && climbFlg == false) // スペースキーが押されたら
            {
                jumpFlg = true; // ジャンプフラグON
            }
            if (jumpFlg == true) // ジャンプフラグがONなら
            {
                myRigidbody.velocity = Vector3.up * jumpSpeed; // ジャンプを移動ベクトルに代入
                jumpCoolDownCount = 0.0f;
                jumpFlg = false; // ２段ジャンプを防ぐため、再度地面に設置しないとジャンプできないようにする
            }
            if (climbFlg == true) // 登り判定がONなら
            {
                myRigidbody.useGravity = false; // 重力OFF
                if (Input.GetAxisRaw("Vertical") > 0.0f)
                {
                    movePos.y += speed;
                    if (movePos.y >= playerMaxSpeed)// 移動ベクトルが最小スピードを下回ったら
                    {
                        movePos.y = playerMaxSpeed;// 移動スピードは最小スピード固定
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0.0f)
                {
                    movePos.y += -speed;
                    if (movePos.y <= playerMinSpeed)// 移動ベクトルが最小スピードを下回ったら
                    {
                        movePos.y = playerMinSpeed;// 移動スピードは最小スピード固定
                    }
                }
                else if (Input.GetAxisRaw("Vertical") == 0 && changeFlg == false && status == STATUS.NONE)
                {
                    movePos.y = 0; // 移動量は０に
                }
            }
            else
            {
                myRigidbody.useGravity = true; // 登り判定OFFで重力ON
            }
        }

        if (movePos.x > 0)
        {
            foreach (var item in uvScroll)
            {
                item.scrollSpeedX = -1.0f;
            }
        }
        else if (movePos.x < 0)
        {
            foreach (var item in uvScroll)
            {
                item.scrollSpeedX = 1.0f;
            }
        }
        else if (movePos.x == 0)
        {
            foreach (var item in uvScroll)
            {
                item.scrollSpeedX = 0.0f;
            }
        }

        if (pageChange.pageChange == true && damageFlg == true)
        {
            // 保存された中間地点に移動する
            gm.SavePosition(transform.position);
            transform.position = gm.GetPosition();
            damageFlg = false;
        }
        else
        {
            gm.SavePosition(transform.position);
            transform.position = gm.GetPosition();
        }

        if (pageChange.pageChange == true && pageChange.pageFlip > -1 && gm.sketchBookValue == gm.tempSketchValue)
        {
            // ページがめくり終わったら残機を減らす
            //damageFlg = true;
        }

        // 移動値を設定
        // ↓↓以下の行よりキー入力によりキャラクター書き換え処理を行う↓↓
        if (changeFlg == false)
        {
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Alpha1)) // ゲームボタン「B」で炎属性に書き換え
            {
                changeFlg = true;
                StatusChenge(STATUS.FIRE);
            }
            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.Alpha2)) // ゲームボタン「X」で水属性に書き換え
            {
                changeFlg = true;
                StatusChenge(STATUS.WATER);
            }
            if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Alpha3)) // ゲームボタン「A」で風属性に書き換え
            {
                changeFlg = true;
                StatusChenge(STATUS.WIND);
            }
            if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown(KeyCode.Alpha4)) // ゲームボタン「Y」で土属性に書き換え
            {
                changeFlg = true;
                StatusChenge(STATUS.EARTH);
            }
        }


        // ↑↑キー入力書き換え処理ここまで↑↑

        // 移動値が入っている場合のみ移動を行う
        CharactorMove(movePos);
    }

    // キャラクター描き換えメソッド
    public override void StatusChenge(STATUS _status)
    {
        SpriteRenderer playerSprite = gameObject.GetComponent<SpriteRenderer>();
        // 変更先のステータスが現在のステータスと同じなら変身しない
        // またページがめくり終わるまで変身できない
        if (status != _status && pageChange.pageFlip < -1)
        {
            switch (_status)
            {
                case STATUS.FIRE:
                    FormChange((int)ANIM_ENUMS.BLUCK.FIRE, _status);
                    break;
                case STATUS.WATER:
                    FormChange((int)ANIM_ENUMS.BLUCK.WATER, _status);
                    break;
                case STATUS.WIND:
                    FormChange((int)ANIM_ENUMS.BLUCK.WIND, _status);
                    break;
                case STATUS.EARTH:
                    FormChange((int)ANIM_ENUMS.BLUCK.STONE, _status);
                    break;
            }
        }
        else if (status == _status && pageChange.pageFlip < -1)
        {

            FormChange((int)ANIM_ENUMS.BLUCK.IDLE, STATUS.NONE);
        }
    }

    //キャラクター移動メソッド
    void CharactorMove(Vector3 pos)
    {
        //キャラクター移動
        transform.position += movePos * Time.deltaTime;
    }

    void OnCollisionEnter(Collision hit)
    {
        // 船と接触したら
        if (hit.gameObject.tag == "Ship")
        {
            shipFlg = true;
        }
        // ダメージオブジェクトに接触したら
        if (hit.gameObject.name == "Needle")
        {
            Debug.Log("ダメージを受けた");
            damageFlg = true;
        }
    }

    /*衝突判定メソッド*/
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Climb") // 登れるオブジェクトに接触したら
        {
            climbFlg = true; // 登り判定ON
        }
    }
    void OnTriggerExit(Collider hit)
    {
        // 上り末端点から離れたら
        if (hit.gameObject.tag == "Climb")
        {
            climbFlg = false; // 登り判定OFF
            myRigidbody.useGravity = true;
        }

    }

    void FormChange(int changeNum, STATUS _status)
    {
        movePos = Vector3.zero;
        statusSr.material.shader = statusMaterial[0].shader;
        transform.DOScale(new Vector3(0, 0, 1), 1.0f).OnComplete(() =>
        {
            statusAnim.SetInteger("BluckAnim", changeNum);
            transform.DOScale(new Vector3(1, 1, 1), 1.0f).OnComplete(() =>
            {
                statusSr.material.shader = statusMaterial[1].shader;
                status = _status;
                changeFlg = false;
            });
        });
    }

    /* 接地判定用メソッド */
    bool GroundJudgment()
    {
        RaycastHit hit; // 衝突判定
        // 線のRayを自キャラの下方向に飛ばす
        var isHit = Physics.Linecast(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y - rayRange, this.transform.position.z), out hit);
        if (isHit) // 衝突していたら
        {
            // 中間地点と接触したら、座標と地面埋まり防止のため、y軸を＋１した値を変数に格納
            if (hit.collider.tag == "WayPoint")
            {
                wayPointPos = hit.collider.transform.position + new Vector3(0.0f, 1.75f, 0.0f);
            }
            if (changeFlg)
            {
                return false;
            }
            // ページをめくり終わるまで、キャラクターは動かないようにする
            if (pageChange.pageFlip > -1)
            {
                return false;
            }
            if (status != STATUS.NONE)
            {
                return false;
            }
            if (damageFlg == true)
            {
                return false;
            }
            if (hit.collider.tag == "Climb")
            {
                return false;
            }
            jumpCoolDownCount += Time.deltaTime;
            // デバッグ用Rayを画面に出力
            Debug.DrawRay(transform.position, Vector3.down * rayRange, Color.red); // デバッグ用に画面にRayを出力
            return true; // 接地している
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            return false; // 接地していない
        }
    }

    void OnCollisionStay(Collision c)
    {
        if(c.gameObject.name == "GroundSlope")
        {
            if(statusAnim.GetInteger("BluckAnim") == 0)
            {
                myRigidbody.constraints = RigidbodyConstraints.FreezePositionX;
            }
            else
            {
                myRigidbody.constraints = RigidbodyConstraints.None;
                myRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

}
