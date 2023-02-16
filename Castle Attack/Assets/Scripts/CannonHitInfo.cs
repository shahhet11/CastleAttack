﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHitInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
//        Debug.Log(collision.gameObject.tag + collision.gameObject.name);
        if ( collision.gameObject.tag == "Ground")
        {
            Blast(collision);
            Destroy(this.gameObject,0.1f);
            return;
        }

        if (collision.gameObject.tag == "Catapult")
        {
            //if (GameManager.instance.CatapultHealthFillbar.fillAmount > 0.1f)
            GameManager.instance.currentMachineryHP -= GameManager.instance.levelCastleDamage;
            
            if (GameManager.instance.currentMachineryHP> 0 && !GameManager.instance.GameOver)
            {
                
                Blast(collision);
                //Debug.Log("CatapultHit0"+ GameManager.instance.CatapultHealthFillbar.fillAmount);
                GameManager.instance.CatapultHealthFillbar.fillAmount = GameManager.instance.CatapultHealthFillbar.fillAmount  - (float)GameManager.instance.levelCastleDamage / GameManager.instance.ThisMachineryHP;
                //Debug.Log("CatapultHit1" + GameManager.instance.CatapultHealthFillbar.fillAmount);
                
            }
            else if(!GameManager.instance.GameOver)
            {
                Blast(collision);
                GameManager.instance.CatapultHealthFillbar.fillAmount = GameManager.instance.CatapultHealthFillbar.fillAmount - (float)GameManager.instance.levelCastleDamage / GameManager.instance.ThisMachineryHP;
                {
                    print("Health is zero");
                    GameManager.instance.Defeat();
                }
                    
                EnemyManager.insance.StopEnemyShooting();
                GameManager.instance.GameOver = true;
                if(GameManager.instance.Player != null)
                {
                    Destroy(GameManager.instance.Player);
                }
                //Defeat
            }
                Destroy(this.gameObject);

        }

    }


    public void Blast(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (collision.gameObject.CompareTag("Catapult") || collision.gameObject.CompareTag("Ground"))
            Instantiate(GameManager.instance.BlastPrefabCannon, pos, rot);

      
    }

}
