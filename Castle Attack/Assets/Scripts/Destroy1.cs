using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy1 : MonoBehaviour {
    public float TimeTODestroy = 0.5f;
    
	// Use this for initialization
	void Start () {
       
    }
    public void DeactivateInSecs()
    {
        Invoke("ABC", TimeTODestroy);
    }
    void ABC()
    {
        gameObject.SetActive(false);
    }
	
}
