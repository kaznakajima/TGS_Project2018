﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public GameObject dialog;
    public GameObject pauseUI;

    void Start()
    {
    }
    public void Pause()//ポーズ
    {
        pauseUI.SetActive(true);
        Pauser.Pause();
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Pauser.Resume();
    }


    public void SelectScene()//セレクトシーンに移動
    {
        StartCoroutine(SingletonMonoBehaviour<ScreenShot>.Instance.SceneChangeShot());
        SceneManager.LoadScene("Stage1_alpha");
    }

    public void DialogButton()//ダイアログ表示
    {
        dialog.SetActive(true);
    }

    public void RetryScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Resume();
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
