using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleMeasurement : MonoBehaviour
{
    private float maxShot = 18f, maxRot = 1.05f;
    public static AngleMeasurement instance;
    [HideInInspector]
    public float temp;
    public trajectoryScript trajectoryScript1;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            if (GameManager.instance.Player != null)
            {
                if (trajectoryScript1 == null)
                {
                    if (GameManager.instance.Player.GetComponent<trajectoryScript>())
                    {
                        trajectoryScript1 = GameManager.instance.Player.GetComponent<trajectoryScript>();
                    }
                    else
                        trajectoryScript1 = GameManager.instance.Player.transform.GetChild(0).GetComponent<trajectoryScript>();
                }
                else if (!trajectoryScript1.isThrown)
                {
                    RotateHand();

                    if (temp > 103)
                    {
                        trajectoryScript1.DotsLimit = false;
                    }
                    else
                    {
                        trajectoryScript1.DotsLimit = true;
                    }
                }
                /*else
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);*/
            }
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
        }
    }

    private void RotateHand()
    {
        if (trajectoryScript1.SHOTX > 0)
        {
           transform.rotation = Quaternion.Euler(0, 0, checkRotationValue());
        }
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
    }

    private float checkRotationValue()
    {
         temp = ((trajectoryScript1.SHOTX * maxRot) / maxShot)* 100 ;
        return temp;
    }
}
