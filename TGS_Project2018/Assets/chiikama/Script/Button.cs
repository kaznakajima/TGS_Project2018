using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public GameObject dialog;
    public GameObject pauseUI;

    // ポーズ中かどうか
    bool isPause;

    // 選択中のボタン
    int buttonState;
    float speed = 1.0f;

    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleTest":
                isPause = true;
                break;
            case "Stage1_alpha":
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
                Resume();
            }

            // 入力判定
            Vector2 inputVec;
            inputVec.y = Input.GetAxisRaw("Vertical");
            float inputY = Input.GetAxisRaw("Vertical") * speed;

            switch (SceneManager.GetActiveScene().name)
            {
                case "TitleTest":
                    // 選択状態の変更
                    if (inputY > 0.0f && buttonState > 0)
                    {
                        buttonState -= 1;
                        Debug.Log(buttonState);
                        // 連続入力防止
                        speed = 0.0f;
                    }
                    else if (inputY < 0.0f && buttonState < 1)
                    {
                        buttonState += 1;
                        // 連続入力防止
                        speed = 0.0f;
                    }
                    // 再入力可能
                    if (inputY == 0)
                    {
                        speed = 1.0f;
                    }

                    if (Input.GetButtonDown("Click"))
                    {
                        switch (buttonState)
                        {
                            case 0:
                                SelectScene();
                                break;
                            case 1:
                                Exit();
                                break;
                        }
                    }
                    break;
                case "Stage1_alpha":
                    // 選択状態の変更
                    if (inputY > 0.0f && buttonState > 0)
                    {
                        buttonState -= 1;
                        Debug.Log(buttonState);
                        // 連続入力防止
                        speed = 0.0f;
                    }
                    else if (inputY < 0.0f && buttonState < 2)
                    {
                        buttonState += 1;
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
                        switch (buttonState)
                        {
                            case 0:
                                Resume();
                                break;
                            case 1:
                                SelectScene();
                                break;
                            case 2:
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


    public void SelectScene()//セレクトシーンに移動
    {
       if(pauseUI != null)
        {
            Resume();
        }
        StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
        SceneManager.LoadScene("SelectTest");
    }

    public void DialogButton()//ダイアログ表示
    {
        dialog.SetActive(true);
    }

    public void RetryScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Resume();
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
