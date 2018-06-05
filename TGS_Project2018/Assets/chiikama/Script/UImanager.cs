﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour {

    private static UImanager instance;
    public static UImanager Instance
    {
        get
        {
            if (instance) return instance;

            instance = FindObjectOfType<UImanager>();

            if (instance) return instance;

            instance = new GameObject("Dialog").AddComponent<UImanager>();

            return instance;
        }
    }

    private Dialog dialog;//ダイアログ表示用キャンバス

	// Use this for initialization

        public bool IsActiveDialogWindow
    {
        get
        {
            return dialog.gameObject.activeSelf;
        }
        set
        {
            dialog.gameObject.SetActive(value);
        }
    }
	
	// Update is called once per frame

    public void DialogSwich()
    {
        IsActiveDialogWindow = !IsActiveDialogWindow;
    }
}
