using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public Animator Character_Animator;
    public Transform AttackPos;
    public float RangeToAttack;
    public LayerMask WhereIsEnemy;
    public static CharacterAttack instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //InvokeRepeating("GamePlayIdle", 0, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.SpawnNew)
        {
            //Debug.Log("GAMEPLAYTHROWANIMATION");
            //Character_Animator.SetInteger("Player", 1);

        }
    }

    public void GamePlayThrowAnimation()
    {
        StartCoroutine(ExecuteGamePlayAnimation());
    }

    IEnumerator ExecuteGamePlayAnimation()
    {
        Character_Animator.SetInteger("Player", 1);
        yield return new WaitForSeconds(0.8f);
        Character_Animator.SetInteger("Player", 0);
    }

    public void OnAttackButtonPressed()
    {
        //Character_Animator.SetInteger("Player", 2);
        //StopCoroutine(MeleeAttack());
        StartCoroutine(MeleeAttack());

    }

    IEnumerator MeleeAttack()
    {
        Character_Animator.SetInteger("Player", 2);
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(AttackPos.position, RangeToAttack, WhereIsEnemy);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyAI>().TakeDamage();
        }
        yield return new WaitForSeconds(0.2f);
        Character_Animator.SetInteger("Player", 0);
    }

     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, RangeToAttack);
    }

   
}
