using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleMeasurement : MonoBehaviour
{
    private float maxShot = 16.5f, maxRot = 1.05f;
    public static AngleMeasurement instance;
    public float temp;
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (GameManager.instance.Player != null)
        {
            if(GameManager.instance.Player.GetComponent<trajectoryScript>())
            {
                RotateHand(GameManager.instance.Player.GetComponent<trajectoryScript>());
            }
            else if(GameManager.instance.Player.transform.GetChild(0).GetComponent<trajectoryScript>())
            {
                RotateHand(GameManager.instance.Player.transform.GetChild(0).GetComponent<trajectoryScript>());
            }

            if (temp > 103)
            {
            GameManager.instance.Player.GetComponent<trajectoryScript>().DotsLimit = false;
            }
            else
            {
                GameManager.instance.Player.GetComponent<trajectoryScript>().DotsLimit = true;
            }
        }

        
    }

    private void RotateHand(trajectoryScript trajectoryScript)
    {
        if (trajectoryScript.SHOTX > 0)
        {
           transform.rotation = Quaternion.Euler(0, 0, checkRotationValue(trajectoryScript));
        }
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
    }

    private float checkRotationValue(trajectoryScript trajectoryScript)
    {
         temp = ((trajectoryScript.SHOTX * maxRot) / maxShot)* 100 ;
        Debug.Log(temp+"TEMPROT");
        return ((trajectoryScript.SHOTX * maxRot) / maxShot)*100;
    }
}
