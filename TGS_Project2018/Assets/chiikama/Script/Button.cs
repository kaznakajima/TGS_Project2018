using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Button : MonoBehaviour
{
    public GameObject dialog;
    public GameObject pauseUI;

    [SerializeField]
    RectTransform selectObj;

    // ポーズ中かどうか
    bool isPause;

    float Interval = 0.0f;

    // 選択中のボタン
    int buttonState;
    float speed = 1.0f;

    // 連続入力防止
    bool onButton = false;
    public static bool selectBack;

    // 自身のAudioSource
    AudioSource myAudio;

    void Start()
    {
        myAudio = GameObject.Find("Audio").gameObject.GetComponent<AudioSource>();
        SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio = SingletonMonoBehaviour<ScreenShot>.Instance.GetBGM();

        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScene":
                isPause = true;
                break;
            case "MainGameScene":
                DOTween.To(() => Interval, volume =>
                 Interval = volume, 1.0f, 1.0f).OnComplete(() =>
                 {
                     SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.Play();
                     DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
                     SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 1.0f, 2.0f);
                 });
                isPause = false;
                break;
        }
    }

    void Update()
    {
        // ポーズ中のみ実行
        if (isPause)
        {
            if (Input.GetButtonDown("Pause"))
            {
                myAudio.PlayOneShot(myAudio.clip);
                Resume();
            }

            // 入力判定
            Vector2 inputVec;
            inputVec.y = Input.GetAxisRaw("Vertical");
            float inputY = Input.GetAxisRaw("Vertical") * speed;

            switch (SceneManager.GetActiveScene().name)
            {
                case "TitleScene":
                    // 選択状態の変更
                    if (inputY > 0.0f && buttonState > 0)
                    {
                        buttonState -= 1;
                        selectObj.anchoredPosition += new Vector2(0, 130);
                        // 連続入力防止
                        speed = 0.0f;
                    }
                    else if (inputY < 0.0f && buttonState < 1)
                    {
                        buttonState += 1;
                        selectObj.anchoredPosition -= new Vector2(0, 130);
                        // 連続入力防止
                        speed = 0.0f;
                    }
                    // 再入力可能
                    if (inputVec.y == 0)
                    {
                        speed = 1.0f;
                    }

                    if (Input.GetButtonDown("Click"))
                    {
                        if (onButton)
                        {
                            return;
                        }

                        switch (buttonState)
                        {
                            case 0:
                                myAudio.PlayOneShot(myAudio.clip);
                                SelectScene("SelectScene");
                                break;
                            case 1:
                                myAudio.PlayOneShot(myAudio.clip);
                                Exit();
                                break;
                        }
                    }
                    break;
                case "MainGameScene":
                    // 選択状態の変更
                    if (inputY > 0.0f && buttonState > 0)
                    {
                        buttonState -= 1;
                        selectObj.anchoredPosition += new Vector2(0, 90);
                        // 連続入力防止
                        speed = 0.0f;
                    }
                    else if (inputY < 0.0f && buttonState < 2)
                    {
                        buttonState += 1;
                        selectObj.anchoredPosition -= new Vector2(0, 90);
                        // 連続入力防止
                        speed = 0.0f;
                    }
                    // 再入力可能
                    if (inputVec.y == 0.0f)
                    {
                        speed = 1.0f;
                    }

                    if (Input.GetButtonDown("Click"))
                    {
                        if (onButton)
                        {
                            return;
                        }

                        switch (buttonState)
                        {
                            case 0:
                                myAudio.PlayOneShot(myAudio.clip);
                                Resume();
                                break;
                            case 1:
                                myAudio.PlayOneShot(myAudio.clip);
                                SelectScene("SelectScene");
                                break;
                            case 2:
                                myAudio.PlayOneShot(myAudio.clip);
                                RetryScene();
                                break;
                        }
                    }
                    break;
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                myAudio.PlayOneShot(myAudio.clip);
                Pause();
            }
        }
       
    }

    public void Pause()//ポーズ
    {
        isPause = true;
        pauseUI.SetActive(true);
        Pauser.Pause();
    }

    public void Resume()
    {
        isPause = false;
        pauseUI.SetActive(false);
        Pauser.Resume();
    }


    public void SelectScene(string selectStr)//セレクトシーンに移動
    {
        selectBack = true;
        onButton = true;
        if (pauseUI != null)
        {
            Resume();
        }
        StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
        DOTween.To(() => SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume, volume =>
        SingletonMonoBehaviour<ScreenShot>.Instance.bgmAudio.volume = volume, 0.0f, 0.5f).OnComplete(() =>
        {
            SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.PlayOneShot(SingletonMonoBehaviour<ScreenShot>.Instance.myAudio.clip);
            SceneManager.LoadScene(selectStr);
        });
    }

    public void DialogButton()//ダイアログ表示
    {
        dialog.SetActive(true);
    }

    public void RetryScene()
    {
        selectBack = false;
        string sceneName = SceneManager.GetActiveScene().name;
        Resume();
        onButton = true;
        StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
        SceneManager.LoadScene(sceneName);
        Debug.Log("りとらい");
        Pauser.Clear();
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void DialogExit()
    {
        dialog.SetActive(false);
    }
}
