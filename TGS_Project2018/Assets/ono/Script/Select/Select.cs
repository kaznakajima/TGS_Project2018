using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Select : MonoBehaviour
{

    [SerializeField, Header("メインカメラ")]
    Camera maincamera;

    [SerializeField, Header("SelectStage_(No.)を入れて")]
    GameObject[] SelectStage;

    [SerializeField, Header("ステージ名")]
    public string stageName;

    public int StageNum = 0; //選択ステージ関数

    bool flg;

    //保存関数
    GameObject scrollObject;
    GameObject rightStage;
    GameObject leftStage;

    [SerializeField, Header("回転速度")]
    float rollSpeed;

    float cameraRotate;

    float Interval = 0.0f;

    // 連続入力防止
    bool onButton = false;
    // ボタンが押せるかどうか
    bool canPush = false;

    float alfa;

    [SerializeField, Header("FadeOut")]
    GameObject fadeImage;

    // 自身のAudioSource
    AudioSource myAudio;

    // ボタン入力
    float speed = 1.0f;

    bool fadeFlg;
    // Use this for initialization
    void Start()
    {
        Camera_Select.flg = false;
        // ステージ情報の初期化
        SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[0];
        // 音楽の設定
        myAudio = GameObject.Find("Audio").gameObject.GetComponent<AudioSource>();
        SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio = SingletonMonoBehaviour<ScreenShot>.Instance.GetBGM();

        // インターバルを計算し、音楽のフェード
        DOTween.To(() => Interval, volume =>
                Interval = volume, 1.0f, 1.0f).OnComplete(() =>
                {
                    canPush = true;
                    SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.Play();
                    DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                    SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 1.0f, 2.0f);
                });

        //alfa = GetComponent<Image>().color.a;

        //maincamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //fadeImage.GetComponent<Image>().color = new Color(0, 0, 0, alfa);

        float selectX = Input.GetAxisRaw("Horizontal") * speed;

        if (selectX > 0 && !fadeFlg)
        {
            Right();
        }
        else if (selectX < 0 && !fadeFlg)
        {
            Left();
        }

        //if (flg == false)
        //{
        //    Scroll(scrollObject, cameraRotate);
        //}
        if (Input.GetButtonDown("Click") && !flg)
        {
            if (onButton || !canPush)
            {
                return;
            }
            myAudio.PlayOneShot(myAudio.clip);
            fadeFlg = true;
            onButton = true;
        }

        // チュートリアルシーンへの移動
        if (Input.GetButtonDown("Tutorial") && !flg && SceneManager.GetActiveScene().name != "Tutorial")
        {
            if (onButton || !canPush)
            {
                return;
            }
            myAudio.PlayOneShot(myAudio.clip);
            onButton = true;
            Scene("Tutorial");
        }
        else if (Input.GetButtonDown("Tutorial") && !flg && SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (onButton || !canPush)
            {
                return;
            }
            myAudio.PlayOneShot(myAudio.clip);
            onButton = true;
            Scene("SelectScene");
        }

        // チュートリアルシーンへの移動
        if (Input.GetButtonDown("Title") && !flg && SceneManager.GetActiveScene().name != "Tutorial")
        {
            if (onButton || !canPush)
            {
                return;
            }
            myAudio.PlayOneShot(myAudio.clip);
            onButton = true;
            Scene("TitleScene");
        }

        if (fadeFlg)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                StageNum = 0;
                SingletonMonoBehaviour<ScreenShot>.Instance.stageNum = 0;
                SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[StageNum];
                Scene(stageName);
                return;
            }
            else
            {
                alfa += 1.0f * Time.deltaTime;
                Camera_Select.flg = true;

                if (alfa >= 1.9)
                {
                    Scene(stageName);
                    fadeFlg = false;
                }
            }
        }

    }
    public void Right()
    {
        if (flg || onButton)
        {
            return;
        }

        // ボタン入力を遮断
        speed = 0.0f;

        if (StageNum < SelectStage.Length - 1)
        {

            StageNum += 1;
            SingletonMonoBehaviour<ScreenShot>.Instance.stageNum = StageNum;

            switch (StageNum)
            {
                //ステージ１
                case 0:
                    leftStage = SelectStage[2];
                    rightStage = SelectStage[1];

                    cameraRotate = 0;

                    // ステージの設定
                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[StageNum];
                    break;
                //ステージ２
                case 1:
                    leftStage = SelectStage[0];
                    rightStage = SelectStage[2];

                    cameraRotate = 120.0f;

                    // ステージの設定
                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[StageNum];
                    break;
                //ステージ３
                case 2:
                    leftStage = SelectStage[1];
                    rightStage = SelectStage[0];

                    cameraRotate = -120.0f;

                    // ステージの設定
                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[StageNum];
                    break;
            }

            scrollObject = rightStage;

            //flg = true;
        }
        Scroll(scrollObject, cameraRotate);
    }

    public void Left()
    {
        if (flg || onButton)
        {
            return;
        }

        // ボタン入力を遮断
        speed = 0.0f;

        if (StageNum > 0)
        {
            StageNum -= 1;
            SingletonMonoBehaviour<ScreenShot>.Instance.stageNum = StageNum;

            switch (StageNum)
            {
                //ステージ１
                case 0:
                    leftStage = SelectStage[2];
                    rightStage = SelectStage[1];

                    cameraRotate = 0;

                    // ステージの設定
                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[StageNum];
                    break;
                //ステージ２
                case 1:
                    leftStage = SelectStage[0];
                    rightStage = SelectStage[2];

                    cameraRotate = 120.0f;

                    // ステージの設定
                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[StageNum];
                    break;
                //ステージ３
                case 2:
                    leftStage = SelectStage[1];
                    rightStage = SelectStage[0];

                    cameraRotate = -120.0f;

                    // ステージの設定
                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[StageNum];
                    break;
            }

            scrollObject = leftStage;

            //flg = true;
        }
        Scroll(scrollObject, cameraRotate);

    }

    void Scroll(GameObject gameObject, float cameraRotate)
    {
        flg = true;
        //Quaternion target = Quaternion.LookRotation(gameObject.transform.position - maincamera.transform.position);
        //maincamera.transform.rotation = Quaternion.RotateTowards(maincamera.transform.rotation, target, rollSpeed * Time.deltaTime);
        maincamera.transform.DORotate(new Vector3(maincamera.transform.rotation.x, cameraRotate, maincamera.transform.rotation.z), 0.5f).OnComplete(() =>
        {
            speed = 1.0f;
            flg = false;
        });
    }

    private IEnumerator TimeStand()
    {
        yield return new WaitForSeconds(2.0f);
        flg = false;
    }

    void Scene(string StageName)
    {
        // 音楽のフェードをしたのちシーン遷移
        DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
        SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.0f, 0.5f).OnComplete(() =>
        {
            StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
            SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
            SceneManager.LoadScene(StageName);
        });
    }
}
