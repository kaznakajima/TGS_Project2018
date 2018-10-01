using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    // テキストの更新するかどうか
    [HideInInspector]
    public bool isUpdate;
    // Playerの参照
    Player player;
    // Mirrorの参照
    Mirror mirror;

	// Use this for initialization
	void Start () {
        GetPlayerAndMirror();
    }

    void GetPlayerAndMirror()
    {
        foreach (var _player in SingletonMonoBehaviour<ScreenShot>.Instance.GetPlayer())
        {
            player = _player;
        }
        foreach (var _mirror in SingletonMonoBehaviour<ScreenShot>.Instance.GetMirror())
        {
            mirror = _mirror;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(player.myElement[0].color.a > 0 &&
            SingletonMonoBehaviour<TextController>.Instance.uiText.text == SingletonMonoBehaviour<TextController>.Instance.scenarioTex[0])
        {
            isUpdate = true;
        }
        if (player.changeFlg &&
            SingletonMonoBehaviour<TextController>.Instance.uiText.text == SingletonMonoBehaviour<TextController>.Instance.scenarioTex[1])
        {
            isUpdate = true;
        }

        if(Goal.clearFlg && 
            SingletonMonoBehaviour<TextController>.Instance.uiText.text == SingletonMonoBehaviour<TextController>.Instance.scenarioTex[2] ||
            Goal.clearFlg &&
            SingletonMonoBehaviour<TextController>.Instance.uiText.text == SingletonMonoBehaviour<TextController>.Instance.scenarioTex[4])
        {
            SingletonMonoBehaviour<TextController>.Instance.currentLine = 5;
            isUpdate = true;
        }

        if (isUpdate)
        {
            TextUpdate();
        }
	}

    void TextUpdate()
    {
        isUpdate = false;
        SingletonMonoBehaviour<TextController>.Instance.SetNextText();
    }
}
