using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// animatorの列挙型を保存するメソッド
/// </summary>

public class ANIM_ENUMS : MonoBehaviour {

    public enum BLUCK
    {
        //Animator遷移方法
        //animator.SetInteger("BluckAnim", (int)ANIM_ENUMS.BLUCK.〇〇); 〇〇は呼び出したいAnimatorの名前
        IDLE = 0,
        RUN_RIGHT=2,
        RUN_LEFT=3,
        DAMAGE=1,
        FIRE=4,
        WATER=5,
        WIND=6,
        STONE=7,
        CLIME=8,
        IDLE_LEFT=9,
        QUESTION_RIGHT=10,
        QUESTION_LEFT=11,
    }

    public enum MIRROR
    {
        //Animator遷移方法
        //animator.SetInteger("BluckAnim", (int)ANIM_ENUMS.MIRROR.〇〇); 〇〇は呼び出したいAnimatorの名前
        IDLE = 0,
        FIRE=1,
        STONE=2,
        WIND=3,
        WATER=4,
        IDLE_RIGHT=5,
    }

    public enum FOREST
    {
        //Animator遷移方法
        //animator.SetInteger("BluckAnim", (int)ANIM_ENUMS.FOREST.〇〇); 〇〇は呼び出したいAnimatorの名前
        IDLE=0,
        BREAK=1,
        FIRE=2,
        RESET,
    }

    public enum STAGECHANGE
    {
        RIGHT=1,
        LEFT=2
    }
}
