using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float movementSpeed = 300;
    
    void Start()
    {
        Destroy(this.gameObject, 1.2f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.SpawnCastleGO.AiEnemySpawnPos.position, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, GameManager.instance.SpawnCastleGO.AiEnemySpawnPos.position) <=1f)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Castle"))
        {
            if(collision.gameObject.GetComponent<EnemyAI>())
                collision.gameObject.GetComponent<EnemyAI>().TakeDamage();
            Destroy(this.gameObject);
        }    
    }
}
