using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMouse : MonoBehaviour
{
    Vector2 direcion;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ArrowPos = transform.position;

        direcion = MousePos - ArrowPos;

        m_FaceMouse();
    }

    void m_FaceMouse()
    {
        transform.right = direcion;
    }
}
