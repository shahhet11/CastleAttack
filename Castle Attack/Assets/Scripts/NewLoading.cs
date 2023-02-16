using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLoading : MonoBehaviour
{
    RectTransform rectTransform;
   
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(new Vector3(0, 0, -75f * Time.deltaTime));

    }
}
