using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilmDrag : MonoBehaviour
{
    // フィルムを持っているかどうか
    public bool hasFilm;

    float MaxRay = 10;

    GameObject Film;
    Vector3 mousePos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // マウスの位置
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 選択中はマウスに追尾
        if (hasFilm)
        {
            Move();

            if (Input.GetMouseButtonUp(0))
            {
                Throw();
            }
        }

        // クリックしたらRayを飛ばす
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hitRay = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, MaxRay);

        if (hitRay.collider)
        {
            // フラグを変更
            hasFilm = !hasFilm;
            Film = hitRay.collider.gameObject;
        }
    }

    void Throw()
    {
        // フラグを変更
        hasFilm = !hasFilm;

        if (Mathf.Abs(Film.transform.position.x - transform.position.x) < 2.5f && !hasFilm)
        {
            Film.GetComponent<Projector_Controller>().Function(gameObject.GetComponent<Projector_Function>());
        }
    }

    void Move()
    {
        // マウスに追尾
        mousePos.z = Film.transform.position.z;
        Film.transform.position = Vector3.Lerp(Film.transform.position, mousePos, 1.0f);
    }

}
