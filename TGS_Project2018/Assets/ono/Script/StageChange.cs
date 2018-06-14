using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageChange : MonoBehaviour {

    //[SerializeField,Header("すてーじいめーじ")]
    //List<Sprite> Stageimage = new List<Sprite>();

    //public GameObject[] Stage;

    //int stageNum;

    //[SerializeField,Header("UIの座標")]
    //List<RectTransform> recttransform = new List<RectTransform>();

    public GameObject stage;

        [SerializeField]
    Animator animator;
	// Use this for initialization
	void Start () {
		//for(int i = 0; i < Stage.Length; i++)
  //      {
  //          recttransform[i] = Stage[i].GetComponent<RectTransform>();
  //      }

        animator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Right()
    {
        animator.SetTrigger("Right");
        StageImageInstance(-1600.0f);
    }

    public void Left()
    {
        animator.SetTrigger("Left");
        StageImageInstance(1600.0f);
    }

    void StageImageInstance(float offset)
    {
        Instantiate(stage,new Vector3(offset,0,0),Quaternion.identity);
    }
}
