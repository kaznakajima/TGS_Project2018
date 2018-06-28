using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Select : MonoBehaviour {

    Camera _camera;

    public static bool flg;
	// Use this for initialization
	void Start () {
        _camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (flg)
        {
            _camera.orthographicSize -= 1.0f * Time.deltaTime;
        }
	}
}
