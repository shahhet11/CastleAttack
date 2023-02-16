using System;
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
    public GameObject bulletPrefeb;
    private Rigidbody2D bullet;
    public float throwSpeed = 300;
    public AudioSource playerVoiceSource, enemyVoiceSource,hitCastelSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerVoiceSource = AddAudioSource(CharacterManager.instance.lstCharactersData[PlayerPrefs.GetInt("Selected_Char")].playerVoiceClip,0.8f);
        enemyVoiceSource = AddAudioSource(CharacterManager.instance.lstCharactersData[PlayerPrefs.GetInt("Selected_Char")].enemyVoiceClip,.6f);
        hitCastelSource = AddAudioSource(CharacterManager.instance.lstCharactersData[PlayerPrefs.GetInt("Selected_Char")].hitCastelClip,.7f);
    }

    private AudioSource AddAudioSource(AudioClip clip,float volume)
    {
        GameObject source= Instantiate(new GameObject(), transform);
        source.AddComponent<AudioSource>();
        source.GetComponent<AudioSource>().playOnAwake = false;
        source.GetComponent<AudioSource>().clip = clip;
        source.GetComponent<AudioSource>().volume = volume;
        return source.GetComponent<AudioSource>();
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
        GameManager.instance.BT_Clik_Sound(true);
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
        if(PlayerPrefs.GetInt("Selected_Char") > 2)// && PlayerPrefs.GetInt("Selected_Char")!=6)
        {
            bullet= Instantiate(bulletPrefeb,AttackPos.position,Quaternion.identity, transform).GetComponent<Rigidbody2D>();
          //  bullet.AddForce((transform.right + transform.up) * throwSpeed);
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
