using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapLoad : MonoBehaviour
{
    // CSVファイル名
    public string CSVName;
    // マップに配置するオブジェクト
    public GameObject[] mapObj;

    // 読み込んだデータを格納する二次元配列
    int[,] stageMapData;
    // 読み込んだデータの二次元配列
    string[,] stageMapArray;

    // 読み込んだデータの行数、列数
    int height = 0, width = 0;

    // 読み込むデータ
    TextAsset CSV_txt;

    void Awake()
    {
        CSV_txt = Resources.Load(CSVName) as TextAsset;

        // readCSVDataでCSVファイルをString型の配列に書き込み、convert2DArrayTypeでそれをint型に変換する
        readCSVData(ref stageMapArray);
        convert2DArrayType(ref stageMapArray, ref stageMapData, height, width);

        // CreateMapでmapChipのオブジェクトを生成する
        CreateMap(stageMapData, height, width);
    }

    void readCSVData(ref string[,] sdata)
    {
        // ストリームリーダーに読み込み
        StringReader sr = new StringReader(CSV_txt.text);
        // ストリングリーダーをstringに変換
        string strString = sr.ReadToEnd();
        // StringSplitOptionを設定(要はカンマとカンマに何もなかったら格納しないことにする)
        StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        // 行に分ける
        string[] lines = strString.Split(new char[] { '\r', '\n' }, option);

        // カンマ分けの準備(区分けする文字の設定)
        char[] spliter = new char[1] { ',' };

        // 行数設定
        int _height = lines.Length;
        // 列数設定
        int _width = lines[0].Split(spliter, option).Length;

        // 返り値の二次元配列の要素数を設定
        sdata = new string[_height, _width];

        // 行データを切り分けて、二次元配列へ変換する
        for (int i = 0;i < _height; i++)
        {
            string[] splitedData = lines[i].Split(spliter, option);
            for(int j = 0;j < _width; j++)
            {
                sdata[i, j] = splitedData[j];
            }
        }

        width = _width;
        height = _height;
    }

    void convert2DArrayType(ref string[,] sarrays, ref int[,] iarrays, int _height, int _width)
    {
        iarrays = new int[_height, _width];
        for(int i = 0; i < _height; i++)
        {
            for(int j = 0;j < _width; j++)
            {
                iarrays[i, j] = int.Parse(sarrays[i, j]);
            }
        }
    }

    void CreateMap(int[,] arrays, int hgt, int wid)
    {
        for (int i = 0; i < wid; i++)
        {
            for (int j = 0; j < hgt; j++)
            {
                if (arrays[j, i] != -1 && arrays[j, i] <= mapObj.Length)
                {

                    Instantiate(mapObj[arrays[j, i]], 
                            transform.position + new Vector3(i, -j, 0),
                            new Quaternion(0.0f, 90.0f, 0.0f, 0.0f));
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
