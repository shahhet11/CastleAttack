using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool isBoat = false;
    float movementSpeed=4.5f;
    bool isDamaged = false;
    bool isReach = false;

    void Update()
    {
        if(isReach)
        {
            if (!isBoat)
                transform.Translate(Vector3.right * (-movementSpeed * Time.deltaTime));
            else
                Invoke("DestroyEnemy", 2);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.storeChar.transform.position,movementSpeed* Time.deltaTime);
            if (transform.position == GameManager.instance.storeChar.transform.position)
                isReach = true;
        }     
    }

    public void TakeDamage()
    {
        if(!isDamaged)
        {
            isDamaged = true;
            Instantiate(GameManager.instance.BloodParticle, transform.position, Quaternion.identity);
            GameManager.instance.PlayerScore +=
                (CharacterManager.instance.lstCharactersData[CharacterManager.instance.characterIndex].BonusRewards * GameManager.instance.coinMul);
            if(isBoat)
                GameManager.instance.Call_Spawn_Enemy();
            Destroy(this.gameObject, 0.21f);
        }
    }

    void DestroyEnemy()
    {
        GameManager.instance.Call_Spawn_Enemy();
        Destroy(this.gameObject);
    }

}
    