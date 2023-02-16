using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float TimeTODestroy = 0.5f;
    public bool canDestroy;
    // Start is called before the first frame update
    void Start()
    {
        if (canDestroy)
            DestroyInSecs();
    }

    public void DestroyInSecs()
    {
        Invoke("ABC", TimeTODestroy);
    }
    void ABC()
    {
        Destroy(this.gameObject);
    }
}
