﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : StatusController {

    /*
     【キャラクター移動クラス】
     操作方法 
     横移動･･･ADキーor左右矢印キー
     ジャンプ･･･Cキー
     属性変換･･･数字キー（1火属性、2水属性、3風属性）
     数字０キーでリセット
     ツタ上り･･･ツタの近く上下矢印キーorWSキー
    */
    // privateでもインスペクター上で編集できるようにSerializeField属性を付けた

    PageChange pageChange; // ページ遷移クラス
    GameMaster gm; // ゲームマスタークラス
    CameraMove cameraMove; // カメラ移動クラス
    UVScroll[] uvScroll = new UVScroll[2]; // 背景スクロールクラス

    ResetController reset;
    ScreenShot sceneCon;

    AudioSource myAudio; // SE

    public SpriteRenderer resetIcon; // リセット用のアイコン

    // 坂道判定
    bool onSlope = false;
    // 滑る判定
    public bool isSlope;
    bool touch = true;

    float Interval;
    [HideInInspector]
    public float rayPoint = 0.85f;
    [SerializeField] float playerSpeed = 1.0f; // キャラクターのスピード
    [SerializeField] float playerMaxSpeed = 1.5f; // プレイヤーの最大スピード
    [SerializeField] float playerMinSpeed = -1.5f; // プレイヤーの最小スピード
    [SerializeField] float speed; // 移動スピード
    [SerializeField] float rayRange; // 接地判定の距離 
    [SerializeField] float rayRangeH = 0.6f; // 水平方向の接地判定距離
    [SerializeField] float edgeJudgeOffset = 0.5f; // ステージ両端を取得するために必要なオフセット値

    [SerializeField] bool isright; // 右を向いているか
    [SerializeField] bool isTouch = false; // 接触しているか
    [SerializeField] bool isGround; // 接地しているか
    [SerializeField] bool climbFlg = false; // 上下移動可能か
    public bool damageFlg = false; // ダメージを受けているか
    public bool changeFlg = false; // 変身しているか
    [SerializeField] bool pageChangeFlg = false; //ページがめくり終わったか
    [SerializeField] string changePageName; // ページ遷移クラスオブジェクトを取得するために必要な文字列型変数

    Rigidbody myRigidbody; // 自分のRigidbody
    public SpriteRenderer[] myElement = new SpriteRenderer[3]; // 属性のアイコン

    public Vector3 movePos; // 移動用変数
    Vector3 inputPos;
    [SerializeField] Vector3 wayPointPos; // リスポーン地点保存用変数
    Vector3 oldVelocity; // なめらかに移動させるために必要な一時保存用ベクトル

    [SerializeField] SpriteRenderer mySprite;
    // Use this for initialization
    void Start() {
        myAudio = GetComponent<AudioSource>();
        statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE); // アニメーションの初期設定
        gm = GameObject.Find("Master").GetComponent<GameMaster>(); // ゲームマスターコンポーネント取得
        uvScroll = FindObjectsOfType<UVScroll>(); // スクロールクラスのコンポーネントを取得
        cameraMove = FindObjectOfType<CameraMove>(); // CameraMobeクラスのコンポーネントを取得
        foreach (var item in uvScroll)
        {
            // スクロール移動の初期設定は0に
            item.scrollSpeedX = 0.0f;
        }
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        pageChange = GameObject.FindObjectOfType<PageChange>(); // ページ遷移のコンポーネント取得
        myRigidbody = this.gameObject.GetComponent<Rigidbody>(); // RigidBodyコンポーネントを取得
        isright = true; // 初期位置では右を向いている
        isSlope = false;
    }

    // Update is called once per frame
    void Update() {
        if (Goal.clearFlg || ResetController.resetIsonFlg)
        {
            foreach (var item in uvScroll)
            {
                // スクロール移動の初期設定は0に
                item.scrollSpeedX = 0.0f;
            }
            return;
        }
        if (isSlope && isTouch)
        {
            isGround = GroundJudgment(); // 常に接地判定を取る
            CharactorMove(movePos);
            return;
        }

        inputPos.x = Input.GetAxisRaw("Horizontal");

        if (gm.sketchBookValue <= 0)
        {
            statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.DAMAGE);
            return;
        }

            isGround = GroundJudgment(); // 常に接地判定を取る
        if (isGround || climbFlg) // プレイヤーが地面に設置しているか登り状態なら
        {
            // なめらかに移動させる
            movePos = Vector3.Lerp(oldVelocity, movePos, playerSpeed * Time.deltaTime);
            oldVelocity = movePos; // なめらかに移動させるために必要な一時保存用ベクトルを保存

            // スティックが右方向に倒れたら
            if (inputPos.x > 0)
            {
                isright = true; // 右判定ON
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.RUN_RIGHT); // 歩行アニメーションON
                movePos.x += speed; // 移動ベクトルにスピードを加算
                if (movePos.x >= playerMaxSpeed) // 移動ベクトルが最大スピードを超えたら
                {
                    movePos.x = playerMaxSpeed; // 移動スピードは最大スピード固定
                }
            }// スティックが左方向に倒れたら
            else if (inputPos.x < 0)
            {
                if (statusAnim.GetInteger("BluckAnim") == 2)
                {
                    movePos.x = 0.0f;
                }

                isright = false; // 右判定OFF
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.RUN_LEFT);
                // 移動ベクトルに負のスピードを加算
                movePos.x += -speed;
                if (movePos.x <= playerMinSpeed)// 移動ベクトルが最小スピードを下回ったら
                {
                    movePos.x = playerMinSpeed;// 移動スピードは最小スピード固定
                }
            }
            // 何も押されていなかったら
            else if (inputPos.x == 0 && changeFlg == false
                && status == STATUS.NONE && ResetController.resetIsonFlg == false)
            {
                movePos.x = 0.0f; // 移動量は０に
                oldVelocity = movePos; // なめらかに移動させるために必要な一時保存用ベクトルを保存

                // 最後の入力キーに応じてアイドルアニメーションを変更
                if (isright == true)
                {
                    // 右を向いていたらアイドルアニメーションは右向き
                    statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE);
                }
                else if (isright == false)
                {
                    // 左を向いていたらアイドルアニメーションは左向き
                    statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE_LEFT);
                }
            }
            // 自キャラの座標x成分 - 1した値がマップの端点を超えておりかつそれ以上移動しようとしたら
            if (this.transform.position.x - edgeJudgeOffset < cameraMove.mapStartX && movePos.x < 0)
            {
                // 移動量は0にする
                movePos.x = 0;
            }
        }

        if (climbFlg == true) // 登り判定がONなら
        {
            RaycastHit hit, hitH, hitHreverse; // 水平方向の衝突判定
            // 線のRayを自キャラの水平方向に飛ばす 向きに応じて左右どちらに飛ばすか決定する
            var isHitH = Physics.Linecast(
                this.transform.position, 
                //new Vector3(isright == true ? this.transform.position.x + rayRangeH : this.transform.position.x - rayRangeH, transform.position.y + rayPoint, this.transform.position.z),
                new Vector3(isright == true ? this.transform.position.x + rayRangeH : this.transform.position.x - rayRangeH,
                this.transform.position.y, this.transform.position.z),
                out hitH);
            var isHitHReverse = Physics.Linecast(
                //this.transform.position, 
                new Vector3(transform.position.x,this.transform.position.y - rayPoint, this.transform.position.z),
                new Vector3(isright == true ? this.transform.position.x + rayRangeH : this.transform.position.x - rayRangeH, transform.position.y - rayPoint, this.transform.position.z),
                out hitHreverse);
            var isHit = Physics.Linecast(
                //this.transform.position, 
                new Vector3(transform.position.x, transform.position.y + rayPoint, this.transform.position.z),
                new Vector3(isright == true ? this.transform.position.x + rayRangeH : this.transform.position.x - rayRangeH,
                this.transform.position.y + rayPoint, this.transform.position.z),
                out hit);
            Debug.DrawLine(new Vector3(isright == true ? this.transform.position.x + rayRangeH : this.transform.position.x - rayRangeH, transform.position.y + rayPoint, this.transform.position.z),
                new Vector3(isright == true ? this.transform.position.x + rayRangeH : this.transform.position.x - rayRangeH,
                this.transform.position.y - rayPoint, this.transform.position.z));
            if ((isHitH || isHitHReverse || isHit) &&  !onSlope)
            {
                // スクロール不可能に
                isTouch = false;
            }
            else if((!isHitH || !isHitHReverse) && !onSlope)
            {
                // 条件に一致していなければスクロール可能
                isTouch = true;
            }

            statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.CLIME);
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
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                movePos.y = 0; // 移動量は０に
            }
        }
        else
        {
            movePos.y = 0.0f; // 移動量のリセット
        }

        // スクロール可能なら移動方向に応じて背景をスクロールさせる
        if (movePos.x > 0 && cameraMove.isScroll)
        {       // 右方向に進んでいたら
            foreach (var item in uvScroll)
            {
                // 左方向にスクロール
                item.scrollSpeedX = -1.0f;
            }
        }
        else if (movePos.x < 0 && cameraMove.isScroll)
        {       // 左方向に進んでいたら
            foreach (var item in uvScroll)
            {
                // 右方向にスクロール
                item.scrollSpeedX = 1.0f;
            }
        }
        else if (movePos.x == 0 || cameraMove.isScroll == false) // キャラクターが静止していたら
        {
            foreach (var item in uvScroll)
            {
                // スクロールの移動量は０
                item.scrollSpeedX = 0.0f;
            }
        }
        // ダメージオブジェクトに接触した状態でページをめくられた場合
        if (pageChange.pageChange == true && damageFlg == true)
        {
            // アニメーションリセット
            statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE);
            transform.position = wayPointPos; // 保存された中間地点に移動する
            damageFlg = false; // ダメージフラグOFF
        }
        else
        {
            gm.SavePosition(transform.position);
            transform.position = gm.GetPosition();
        }

        // 移動値を設定
        // ↓↓以下の行よりキー入力によりキャラクター書き換え処理を行う↓↓
        if (changeFlg == false && !climbFlg)
        {
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.RightArrow)) // ゲームボタン「B」で炎属性に書き換え
            {
                StatusChenge(STATUS.FIRE);
            }
            if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftArrow)) // ゲームボタン「X」で水属性に書き換え
            {
                StatusChenge(STATUS.WATER);
            }
            //if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.DownArrow) ) // リセット用
            //{
            //    StatusChenge(STATUS.TRADE);
            //}
            if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown(KeyCode.UpArrow)) // ゲームボタン「Y」で土属性に書き換え
            {
                StatusChenge(STATUS.WIND);
                //changeFlg = true;
                //StatusChenge(STATUS.EARTH);
            }

        }
        // ↑↑キー入力書き換え処理ここまで↑↑

        // 移動値が入っている場合のみ移動を行う
        CharactorMove(movePos);
    }

    // キャラクター描き換えメソッド
    public override void StatusChenge(STATUS _status)
    {
        foreach (var IconAlpha in myElement)
        {
            if(IconAlpha.color.a <= 0)
            {
                return;
            }
        }
        // ページがめくり終わるまで変身できない
        if (status != _status && pageChange.pageFlip < -1)
        {
            changeFlg = true;
            switch (_status)
            {
                case STATUS.FIRE: // 炎属性に変身
                    FormChange((int)ANIM_ENUMS.BLUCK.FIRE, _status);
                    break;
                case STATUS.WATER: // 水属性に変身
                    FormChange((int)ANIM_ENUMS.BLUCK.WATER, _status);
                    break;
                case STATUS.WIND: // 風属性に変身
                    FormChange((int)ANIM_ENUMS.BLUCK.WIND, _status);
                    break;
                case STATUS.TRADE: // 土属性に変身
                    if (statusAnim.GetInteger("BluckAnim") == 0 || statusAnim.GetInteger("BluckAnim") == 2)
                    {
                        FormChange((int)ANIM_ENUMS.BLUCK.QUESTION_RIGHT, _status);
                    }
                    else if (statusAnim.GetInteger("BluckAnim") == 3 || statusAnim.GetInteger("BluckAnim") == 9)
                    {
                        FormChange((int)ANIM_ENUMS.BLUCK.QUESTION_LEFT, _status);
                    }
                    break;
            }
        }// 変更先のステータスが現在のステータスと同じなら元のキャラクターに戻る
        else if (status == _status && pageChange.pageFlip < -1)
        {
            changeFlg = true;
            FormChange((int)ANIM_ENUMS.BLUCK.IDLE, STATUS.NONE);
        }
    }

    //キャラクター移動メソッド
    void CharactorMove(Vector3 pos)
    {  
        if (isSlope)
        {
            if(status != STATUS.NONE)
            {
                return;
            }

            if(IceGimmick.moveX > 0)
            {
                pos.x = 5.0f;
            }else if(IceGimmick.moveX < 0)
            {
                pos.x = -5.0f;
            }
        }
        if (!isTouch || damageFlg)
        {
            pos.x = 0.0f;
        }
      
        transform.position += pos * Time.deltaTime;
    }

    void OnCollisionEnter(Collision hit)
    {
        // ダメージオブジェクトに接触したら
        if (hit.gameObject.tag == "Needle" || hit.gameObject.tag == "Water" && damageFlg == false)
        {
            ResetController.resetIsonFlg = false;

            // ダメージ音
            hit.gameObject.GetComponent<AudioSource>().PlayOneShot(hit.gameObject.GetComponent<AudioSource>().clip);

            SingletonMonoBehaviour<ResetController>.Instance.canReset = true;
            Interval = 0.0f;

            // ダメージアニメーション再生
            statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.DAMAGE);
            damageFlg = true; // ダメージフラグON

            // インターバル分待ってからリセット
            DOTween.To(() => Interval, volume =>
              Interval = volume, 0.25f, 1.0f).OnComplete(() =>
              {
                  Button.selectBack = false;
                  StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                  StartCoroutine(pageChange.ScreenShot());
                  Interval = 0.0f;
                  DOTween.To(() => Interval, volume =>
                   Interval = volume, 0.25f, 0.5f).OnComplete(() =>
                  {
                      if (gm.sketchBookValue > 0 && Interval == 0.25f)
                      {
                          FormChange((int)ANIM_ENUMS.BLUCK.IDLE, STATUS.NONE);
                          statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE);
                          if (SingletonMonoBehaviour<ResetController>.Instance.canReset && hit.collider.gameObject.layer != 8)
                          {
                              transform.position = wayPointPos; // 保存された中間地点に移動する
                          }
                          damageFlg = false; // ダメージフラグOFF
                          Interval = 0.0f;
                      }
                  });
              });
        }
    }

    // プレイヤーが坂の上に立ったら
    void OnCollisionStay(Collision c)
    {
        if (c.gameObject.name == "GroundSlope")
        {
            onSlope = true;
            rayPoint = 0.0f;
            // 止まっているなら滑る
            if (statusAnim.GetInteger("BluckAnim") == 0 || statusAnim.GetInteger("BluckAnim") == 9)
            {
                myRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            // 歩いているなら滑らない
            else
            {
                myRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
        if(c.gameObject.name == "Ground")
        {
            movePos.y = 0.0f;
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                   RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void OnCollisionExit(Collision c)
    {
        if (c.gameObject.name == "Ground" && isSlope == true)
        {
            wayPointPos = new Vector3(c.gameObject.transform.position.x, c.gameObject.transform.position.y + 2.0f, 0.0f);
        }

        if(c.gameObject.name == "Ice")
        {
            isSlope = false;
        }

        if (c.gameObject.name == "GroundSlope")
        {
            onSlope = false;
            rayPoint = 0.85f;
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                     RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    /*衝突判定メソッド*/
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Climb") // 登れるオブジェクトに接触したら
        {
            climbFlg = true; // 登り判定ON
            myRigidbody.useGravity = false;
        }
    }
    void OnTriggerExit(Collider hit)
    {
        // 登るオブジェクトから離れたら
        if (hit.gameObject.tag == "Climb")
        {
            climbFlg = false; // 登り判定OFF
            movePos.y = 0.0f; // 移動量のリセット
            myRigidbody.useGravity = true; // 重力ON

            // オブジェクトが離れた際の向きに応じてアニメーションの向きも変える
            if (isright == true)
            {
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.RUN_RIGHT);
            }
            else if (isright == false)
            {
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.RUN_LEFT);
                movePos = Vector3.zero;
            }
        }
    }

    public void FormChange(int changeNum, STATUS _status)
    {
        movePos = Vector3.zero;
        statusSr.material.shader = statusMaterial[0].shader;

        if(_status != STATUS.NONE)
        {
            myAudio.PlayOneShot(myAudio.clip);
        }

        transform.DOScale(new Vector3(0, 0, 1), 1.0f).OnComplete(() =>
        {
            statusAnim.SetInteger("BluckAnim", changeNum);
            transform.DOScale(new Vector3(1, 1, 1), 1.0f).OnComplete(() =>
            {
                changeFlg = false;
                statusSr.material.shader = statusMaterial[1].shader;
                status = _status;
            });
        });
    }

    /* 接地判定用メソッド */
    bool GroundJudgment()
    {
        if (climbFlg)
        {
            return false;
        }

        RaycastHit hitV; // 下方向の衝突判定
        // 線のRayを自キャラの下方向に飛ばす
        var isHit = Physics.Linecast(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y - rayRange, this.transform.position.z), out hitV);

        RaycastHit hitH; // 水平方向の衝突判定
        // 線のRayを自キャラの水平方向に飛ばす 向きに応じて左右どちらに飛ばすか決定する
        var isHitH = Physics.Linecast(
            //this.transform.position, 
            new Vector3(transform.position.x, transform.position.y - rayPoint, 0.0f),
            new Vector3(isright == true ? this.transform.position.x + rayRangeH : this.transform.position.x - rayRangeH,
            this.transform.position.y - rayPoint, this.transform.position.z),
            out hitH);
        // 水平方向のRayがオブジェクトに接触かつプレイヤーが移動中なら
        if (isHitH && !onSlope)
        {
            // スクロール不可能に
            isTouch = false;
        }
        else
        {
            // 条件に一致していなければスクロール可能
            isTouch = true;
        }

        if (isHit) // 衝突していたら
        {
            // 中間地点と接触したら、座標と地面埋まり防止のため、y軸を＋１した値を変数に格納
            if (hitV.collider.tag == "WayPoint")
            {
                wayPointPos = hitV.collider.transform.position + new Vector3(0.0f, 1.75f, 0.0f);
            }
            if(hitV.collider.tag == "Needle" || hitV.collider.tag == "Water")
            {
                return false;
            }
            // 変身中は動かない
            if (changeFlg)
            {
                return false;
            }
            // ページをめくり終わるまで、キャラクターは動かないようにする
            if (pageChange.pageFlip > -1)
            {
                return false;
            }
            // 何らかの属性に変身していたらキャラクターは動かない
            if (status != STATUS.NONE)
            {
                return false;
            }
            // ダメージオブジェクトに当たったらキャラクターは動かない
            if (damageFlg == true)
            {
                movePos = Vector3.zero;
                return false;
            }
            // ツタオブジェクトに当たったら一時的に接地判定無効化
            if (hitV.collider.tag == "Climb")
            {
                return false;
            }
            if (Goal.clearFlg == true)
            {
                movePos = Vector3.zero;
                statusAnim.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.IDLE);
                return false;
            }
            // デバッグ用Rayを画面に出力
            Debug.DrawRay(transform.position, Vector3.down * rayRange,Color.red); // デバッグ用に画面にRayを出力
            // Ray確認用デバッグ
            if (isright)
            {
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 1.0f, 0.0f), Vector3.right * rayRangeH, Color.red);
            }
            else
            {
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 1.0f, 0.0f), Vector3.left * rayRangeH, Color.red);
            }
            return true; // 各条件に該当していなければ移動可能
        }
        else 
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            return false; // 接地していない（移動不可）
        }
    }

}
