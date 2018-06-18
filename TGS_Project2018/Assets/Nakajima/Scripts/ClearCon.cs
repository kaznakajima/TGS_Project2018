using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearCon : MonoBehaviour
{
    // セレクト状態
    [SerializeField]
    RectTransform selectObj;

    // 現在のステート
    int state;

    // 自身のAudioSource
    AudioSource myAudio;

    // ボタン入力
    float speed = 1.0f;

    bool fadeFlg;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ボタン入力
        float selectX = Input.GetAxisRaw("Horizontal") * speed;
        if(selectX > 0)
        {
            state = 1;
            selectObj.anchoredPosition = new Vector2(125.0f, selectObj.anchoredPosition.y);
        }
        else if(selectX < 0)
        {
            state = -1;
            selectObj.anchoredPosition = new Vector2(-435.0f, selectObj.anchoredPosition.y);
        }

        if (Input.GetButtonDown("Click"))
        {
            switch (state)
            {
                case 0:
                    StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                    SceneManager.LoadScene("SelectTest");
                    break;
                case 1:
                    StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                    SingletonMonoBehaviour<ScreenShot>.Instance.stageNum += 1;
                    SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[SingletonMonoBehaviour<ScreenShot>.Instance.stageNum];
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
            }
        }
    }
}
