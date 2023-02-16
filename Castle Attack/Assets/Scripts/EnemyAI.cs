using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        //movementSpeed = 2;
        

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * -movementSpeed * Time.deltaTime);
    }

    public void TakeDamage()
    {

        //Debug.Log("DamageTakemn");
        Instantiate(GameManager.instance.BloodParticle,transform.position, Quaternion.identity);
        GameManager.instance.PlayerScore += CharacterManager.instance.lstCharactersData[CharacterManager.instance.characterIndex].BonusRewards;
        
        
        Destroy(this.gameObject,0.21f);
    }

   

}
