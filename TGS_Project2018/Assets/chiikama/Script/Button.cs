using System.Collections;
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
        SceneManager.LoadScene("Select");
    }

    public void DialogButton()//ダイアログ表示
    {
        dialog.SetActive(true);
    }

    public void RetryScene()
    {
        
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        Debug.Log("りとらい");
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
