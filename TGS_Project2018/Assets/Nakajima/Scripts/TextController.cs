using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    // テキストの格納
    public string[] scenarioTex;
    // uiTextへの参照
    [SerializeField]
    Text uiText;
    // 読み込むテキストファイル
    [SerializeField]
    TextAsset loadAsset;
    // 読み込んだテキストデータ
    string loadText;

    [SerializeField] [Range(0.001f, 0.3f)]
    // 1文字の表示にかかる時間
    float intervalDisplay = 0.05f;

    // 現在の行番号
    int currentLine = 0;
    // 現在の文字列
    private string currentText = string.Empty;
    // 表示にかかる時間
    private float timeDisplay = 0;
    // 文字列の表示を開始した時間
    private float timeElapsed = 1;
    // 表示中の文字数
    private int lastUpdateText = -1;

    // 文字の表示が完了しているかどうか
    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeDisplay; }
    }

	// Use this for initialization
	void Start () {
        loadText = loadAsset.text;
        scenarioTex = loadText.Split(char.Parse("\n"));
	}
	
	// Update is called once per frame
	void Update () {
        // 現在の行番号がラストまで行っていない状態でクリックすると、テキストを更新する
        if (IsCompleteDisplayText)
        {
            if (currentLine < scenarioTex.Length && Input.GetButtonDown("Click"))
            {
                SetNextText();
            }
        }
        else
        {
            // 完了していないなら文字をすべて表示する
            if (Input.GetButtonDown("Click"))
            {
                timeDisplay = 0;
            }
        }
        

        // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
        int displayTextCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeDisplay) * currentText.Length);

        // 表示文字数が前回の表示文字数と異なるならテキストを表示する
        if(displayTextCount != lastUpdateText)
        {
            uiText.text = currentText.Substring(0, displayTextCount);
            lastUpdateText = displayTextCount;
        }
	}

    void SetNextText()
    {
        // 現在のテキストをuiTextに流し込み、現在の行番号を一つ追加する
        currentText = scenarioTex[currentLine];

        // 想定表示時間と現在の時刻をキャッシュ
        timeDisplay = currentText.Length * intervalDisplay;
        timeElapsed = Time.time;
        currentLine++;

        // 文字カウントを初期化
        lastUpdateText = -1;
    }
}
