using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{

    // ステータス一覧
    public enum STATUS
    {
        NONE,
        FIRE,
        WATER,
        WIND,
        EARTH
    }

    // 現在のステータス
    public STATUS status;

    // 自分のSpriteRenderer
    public SpriteRenderer statusSr;

    // 自分のAnimator
    public Animator statusAnim;

    // 変化用のマテリアル
    public Material[] statusMaterial;

    // ステータスに応じて姿を変えるメソッド
    // 使い方(継承先)　public override void StatusChange(STATUS 引数名)
    //                         {
    //                             ここに処理を書く    
    //                         }
    public virtual void StatusChenge(STATUS _status)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
