using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTrajectoryMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    public float degree=160;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //TrackMovement();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TrackMovement();
    }
    void TrackMovement()
    {
        Vector2 direction = rigid.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - degree, Vector3.forward);

        
    }

}
