using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHitInfo : MonoBehaviour
{
    public Transform target;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "Ground")
        {
            Blast(collision);
            Destroy(this.gameObject,0.1f);
            return;
        }

        if (collision.gameObject.tag == "Catapult")
        {
            GameManager.instance.currentMachineryHP -= GameManager.instance.levelCastleDamage;
            
            if (GameManager.instance.currentMachineryHP> 0 && !GameManager.instance.GameOver)
            {
                Blast(collision);
                GameManager.instance.CatapultHealthFillbar.fillAmount = GameManager.instance.CatapultHealthFillbar.fillAmount  - (float)GameManager.instance.levelCastleDamage / GameManager.instance.ThisMachineryHP;  
            }
            else if(!GameManager.instance.GameOver)
            {
                Blast(collision);
                GameManager.instance.CatapultHealthFillbar.fillAmount = GameManager.instance.CatapultHealthFillbar.fillAmount - (float)GameManager.instance.levelCastleDamage / GameManager.instance.ThisMachineryHP;
                {
                    GameManager.instance.Defeat();
                }
                    
                EnemyManager.insance.StopEnemyShooting();
                GameManager.instance.GameOver = true;
                if(GameManager.instance.Player != null)
                {
                    Destroy(GameManager.instance.Player);
                }
            }
            Destroy(this.gameObject);
        }
    }

    public void Blast(Collision2D collision)
    {
        if (Sound_Manager.instance)
            Sound_Manager.instance.enemyHit.Play();

        ContactPoint2D contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (collision.gameObject.CompareTag("Catapult") || collision.gameObject.CompareTag("Ground"))
            Instantiate(GameManager.instance.BlastPrefabCannon, pos, rot);
    }

    internal void Shoot(float LaunchForce)
    {
       target = GameManager.instance.playerMachine.transform;

        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right * LaunchForce * -1);
        }
        else
            transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(transform.right * LaunchForce * -1);
    }
}
