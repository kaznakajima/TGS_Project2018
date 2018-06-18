using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
    // スクロールの大きさ
    [SerializeField]
    public float scrollSpeedX = 1.0f;

    void Start()
    {
       
    }
    void Update()
    {
        transform.Translate(scrollSpeedX * Time.deltaTime, 0, 0);
        CheckCameraOut(transform.position, scrollSpeedX);
    }

    // カメラ範囲内判定
    void CheckCameraOut(Vector3 _pos, float moveX)
    {
        // カメラ範囲内座標 (0.0f ～ 1.0f)
        Vector3 view_pos = Camera.main.WorldToViewportPoint(_pos);

        // カメラの右端
        if (moveX > 0 && view_pos.x > 1.5f)
        {
            transform.position = new Vector2(transform.parent.position.x - 13.34f, transform.parent.position.y);
        }
        // カメラの左端
        else if (moveX < 0 && view_pos.x < -0.5f)
        {
            transform.position = new Vector2(transform.parent.position.x + 13.34f, transform.parent.position.y);
        }
    }
}
