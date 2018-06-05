using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projector_Function : MonoBehaviour
{

    bool isplayer;
    bool animStop;

    [HideInInspector]
    public float animSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
       
	}

    public bool isPlayer
    {
        set
        {
            isplayer = value;
        }
        get
        {
            return isplayer;
        }
    }

    public bool AnimStop
    {
        set
        {
            animStop = value;
            if (animStop)
            {
                animSpeed = 1.0f;
            }
            else
            {
                animSpeed = 0.0f;
            }
        }
        get
        {
            return animStop;
        }
    }

    //void OnTriggerStay2D(Collider2D collider)
    //{
    //    if (film.hasFilm == false)
    //    {
    //        collider.gameObject.GetComponent<Projector_Controller>().Function(this);
    //    }
    //}
}
