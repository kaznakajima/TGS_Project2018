using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{ 

    void Start()
    {
    }
    public void PauseUI()//ポーズ
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

            Pauser.Pause();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Pauser.Resume();
        }
    }


    public void SelectScene()//セレクトシーンに移動
    {
        SceneManager.LoadScene("Select");
    }

    public void DialogButton()//ダイアログ表示
    {
        UImanager.Instance.DialogSwich();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
