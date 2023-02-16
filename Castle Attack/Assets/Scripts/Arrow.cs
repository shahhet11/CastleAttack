using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float releaseDelay;
    private bool isPressed;
    private float maxDragDistance = 2f;
    private Rigidbody2D SlingRb;
    private Rigidbody2D rb;
    private SpringJoint2D sj;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
        SlingRb = sj.connectedBody;
        releaseDelay = 1 / (sj.frequency * 4);
    }

    void Update()
    {
        if (isPressed)
            DragArrow();
    }
    private void OnMouseDown()
    {
        isPressed = true;
        rb.isKinematic = true;

    }
    private void OnMouseUp()
    {
        isPressed = false;
        rb.isKinematic = false;
        StartCoroutine(Release());

    }
    private void DragArrow()
    {
        Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //rb.position = mouseposition;

        float distance = Vector2.Distance(mouseposition, SlingRb.position);
        if (distance > maxDragDistance)
        {
            Vector2 direction = (mouseposition - SlingRb.position).normalized * -1 ;
            rb.position = SlingRb.position + direction * maxDragDistance;
        }
        else
        {
            rb.position = mouseposition;
        }

    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseDelay);
        sj.enabled = false;
    }

}
