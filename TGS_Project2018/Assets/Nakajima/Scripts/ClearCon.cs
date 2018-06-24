using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ClearCon : MonoBehaviour
{
    // セレクト状態
    [SerializeField]
    RectTransform selectObj;

    // 現在のステート
    int state;

    // ボタン入力
    float speed = 1.0f;

    // インターバル
    float Interval = 0.0f;

    // Use this for initialization
    void Start()
    {
        SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio = SingletonMonoBehaviour<ScreenShot>.Instance.GetBGM();

        DOTween.To(() => Interval, volume =>
                Interval = volume, 1.0f, 1.0f).OnComplete(() =>
                {
                    SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.Play();
                    DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                    SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 1.0f, 1.0f);
                });
    }

    // Update is called once per frame
    void Update()
    {
        // ボタン入力
        float selectX = Input.GetAxisRaw("Vertical") * speed;
        if(selectX > 0)
        {
            state = 0;
            selectObj.anchoredPosition = new Vector2(95.0f, selectObj.anchoredPosition.y);
        }
        else if(selectX < 0)
        {
            if(SceneManager.GetActiveScene().name == "Stage1_alpha" && SingletonMonoBehaviour<ScreenShot>.Instance.stageNum == 2)
            {
                return;
            }
            state = 1;
            selectObj.anchoredPosition = new Vector2(-45.0f, selectObj.anchoredPosition.y);
        }

        if (Input.GetButtonDown("Click"))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "GameOver":
                    switch (state)
                    {
                        case 0:
                            StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                                SceneManager.LoadScene("SelectTest");
                            });
                            break;
                        case 1:
                            StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                                SceneManager.LoadScene("Stage1_alpha");
                            });
                            break;
                    }

                    
                    break;
                case "Stage1_alpha":
                    switch (state)
                    {
                        case 0:
                            StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                                SceneManager.LoadScene("SelectTest");
                            });
                            break;
                        case 1:
                            StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                            SingletonMonoBehaviour<ScreenShot>.Instance.stageNum += 1;
                            Debug.Log(SingletonMonoBehaviour<ScreenShot>.Instance.stageNum);
                            SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[SingletonMonoBehaviour<ScreenShot>.Instance.stageNum];
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            });
                            break;
                    }
                    break;
            }
        }
    }
}
