using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Select : MonoBehaviour {

    [SerializeField,Header("メインカメラ")]
    Camera maincamera;

    [SerializeField,Header("SelectStage_(No.)を入れて")]
    GameObject[] SelectStage;

    [SerializeField,Header("ステージ名")]
    public string stageName;

   public int StageNum = 0; //選択ステージ関数

    bool flg;

    //保存関数
    GameObject scrollObject;
    GameObject rightStage;
    GameObject leftStage;

    [SerializeField,Header("回転速度")]
    float rollSpeed;

    float cameraRotate;

    float alfa;

    [SerializeField,Header("FadeOut")]
    GameObject fadeImage;

    // ボタン入力
    float speed = 1.0f;

    bool fadeFlg;
    // Use this for initialization
    void Start () {
        fadeFlg = false;
        Camera_Select.flg = false;
        SingletonMonoBehaviour<ScreenShot>.Instance.csvName = "Test";

        //alfa = GetComponent<Image>().color.a;

        //maincamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //fadeImage.GetComponent<Image>().color = new Color(0, 0, 0, alfa);

        // ボタン入力
        float selectX = Input.GetAxisRaw("Horizontal") * speed;

        if(selectX > 0)
        {
            Right();
        }else if(selectX < 0)
        {
            Left();
        }

        //if (flg == false)
        //{
        //    Scroll(scrollObject, cameraRotate);
        //}
        if (Input.GetButtonDown("Click"))
        {
            fadeFlg = true;
        }

        if (fadeFlg)
        {
            alfa += 0.5f * Time.deltaTime;
            Camera_Select.flg = true;

            if (alfa >= 1)
            {
                StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                Scene(stageName);
            }
        }
       
    }
    public void Right()
    {
        if (flg)
        {
            return;
        }

        // ボタン入力を遮断
        speed = 0.0f;

        int a = SelectStage.Length / 2;
        Debug.Log(StageNum);
       
        if(StageNum < SelectStage.Length - 1)
        {

            StageNum += 1;

            switch (StageNum)
            {
                //ステージ１
                case 0:
                    leftStage = SelectStage[2];
                    rightStage = SelectStage[1];

                    cameraRotate = 0;

                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = "Test";
                    break;
                //ステージ２
                case 1:
                    leftStage = SelectStage[0];
                    rightStage = SelectStage[2];

                    cameraRotate = 120.0f;

                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = "Test_2";
                    break;
                //ステージ３
                case 2:
                    leftStage = SelectStage[1];
                    rightStage = SelectStage[0];

                    cameraRotate = -120.0f;

                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = "1-1";
                    break;
            }

            scrollObject = rightStage;

            //flg = true;
        }
        Scroll(scrollObject, cameraRotate);
    }

    public void Left()
    {
        if (flg)
        {
            return;
        }

        // ボタン入力を遮断
        speed = 0.0f;

        int a = SelectStage.Length / 2;

        Debug.Log(StageNum);

       if(StageNum > 0)
        {
            StageNum -= 1;

            switch (StageNum)
            {
                //ステージ１
                case 0:
                    leftStage = SelectStage[2];
                    rightStage = SelectStage[1];

                    cameraRotate = 0;
                    break;
                //ステージ２
                case 1:
                    leftStage = SelectStage[0];
                    rightStage = SelectStage[2];
                    cameraRotate = 120.0f;
                    break;
                //ステージ３
                case 2:
                    leftStage = SelectStage[1];
                    rightStage = SelectStage[0];
                    cameraRotate = -120.0f;
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
        SceneManager.LoadScene(StageName);
    }
}
