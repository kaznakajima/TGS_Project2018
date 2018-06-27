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

    // 自身のAudioSource
    AudioSource myAudio;

    // Use this for initialization
    void Start()
    {
        // BGMの設定
        myAudio = GameObject.Find("Audio").gameObject.GetComponent<AudioSource>();
        SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio = SingletonMonoBehaviour<ScreenShot>.Instance.GetBGM();

        // 音楽のフェード
        DOTween.To(() => Interval, volume =>
                Interval = volume, 1.0f, 1.0f).OnComplete(() =>
                {
                    SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.Play();
                    DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                    SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 1.0f, 1.0f);
                    Button.selectBack = false;
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
            // ステージが最大だったら入力をキャンセル
            if(SceneManager.GetActiveScene().name == "MainGameScene" && SingletonMonoBehaviour<ScreenShot>.Instance.stageNum == 2)
            {
                return;
            }
            state = 1;
            selectObj.anchoredPosition = new Vector2(-45.0f, selectObj.anchoredPosition.y);
        }

        if (Input.GetButtonDown("Click"))
        {
            if (Button.selectBack)
            {
                return;
            }
            myAudio.PlayOneShot(myAudio.clip);
            Button.selectBack = true;
            switch (SceneManager.GetActiveScene().name)
            {
                case "GameOver":
                    switch (state)
                    {
                        case 0:
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                                SceneManager.LoadScene("SelectScene");
                            });
                            break;
                        case 1:
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                                SceneManager.LoadScene("MainGameScene");
                            });
                            break;
                    } 
                    break;
                case "MainGameScene":
                    switch (state)
                    {
                        case 0:
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
                                SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
                                SceneManager.LoadScene("SelectScene");
                            });
                            break;
                        case 1:
                            SingletonMonoBehaviour<ScreenShot>.Instance.stageNum += 1;
                            SingletonMonoBehaviour<ScreenShot>.Instance.csvName = SingletonMonoBehaviour<ScreenShot>.Instance.csvData[SingletonMonoBehaviour<ScreenShot>.Instance.stageNum];
                            DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                            SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.5f, 1.0f).OnComplete(() =>
                            {
                                StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
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
