using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTrajectoryMovement : MonoBehaviour
{

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //TrackMovement();
    }

    // Update is called once per frame
    void Update()
    {
        TrackMovement();
    }
    void TrackMovement()
    {
        Vector2 direction = transform.GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 160, Vector3.forward);
    }

}
