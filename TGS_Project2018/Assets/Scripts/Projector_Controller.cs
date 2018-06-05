using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projector_Controller : MonoBehaviour
{
    public enum FunctionType
    {
        WATER,
        FIRE,
        SUNDER
    }

    [SerializeField]
    FunctionType type;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    Animator PlayerAnim;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 回転
            Vector3 axis = new Vector3(0.0f, 0.0f, 1.0f);
            float angle = 180.0f * Time.deltaTime;
            Quaternion _quaternion = Quaternion.AngleAxis(angle, axis);
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, _quaternion * Player.transform.rotation, 10.0f);
        }
    }

    public void Function(Projector_Function projector)
    {
        switch (type)
        {
            case FunctionType.WATER:
                Player.SetActive(projector.isPlayer);
                projector.isPlayer = !projector.isPlayer;
                break;
            default:
                PlayerAnim.SetFloat("AnimSpeed", projector.animSpeed);
                projector.AnimStop = !projector.AnimStop;
                break;
        }
        Debug.Log(type);
        Destroy(gameObject);
    }
}
