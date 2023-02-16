using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public AudioSource CannonSFX;
    public AudioClip[] WeaponShootClip;
    public GameObject EnemyAmmos;
    public Transform[] EnemyMuzzlePos;
    public float LaunchForce = 550;
    public static EnemyManager insance;
    public int RandomMiss;
    public int CannonIndex;
    public GameObject DustSpawn;

    public GameObject[] EnemySpawn;
    public Animator[] CastleMachineryAnimator;

    public LevelsData.WeaponType _WeaponType;
    // Start is called before the first frame update
    private void Awake()
    {
        insance = this;
    }
    void Start()
    {
       
       // StartEnemyShooting();
    }
    void InitiateShoot()
    {
       StartCoroutine(EnemyShoot());
    }
    IEnumerator EnemyShoot()
    {
        RandomMiss += 1;
        
        if (RandomMiss % 3 == 0 && RandomMiss != 0)
        {
            //Debug.Log("MiSS");
            int r = Random.Range(100, 400);
            LaunchForce = r;
        }
        else
        {
            int r;
            //Debug.Log("Hit");
            if(CannonIndex == 1)
            {
                r = Random.Range(200, 220);
                LaunchForce = r;
               
            }
            else
            {
             r = Random.Range(200, 220);
             LaunchForce = r;
            }
        }

        if (_WeaponType == LevelsData.WeaponType.Arrow || _WeaponType == LevelsData.WeaponType.FireArrow)
        {
           // Debug.Log("ARROWSENEMY");
            StartCoroutine(EnemyShootArrow());
            if (_WeaponType == LevelsData.WeaponType.Arrow)
            {
                CannonSFX.clip = WeaponShootClip[2];
                
            }
            else if (_WeaponType == LevelsData.WeaponType.FireArrow)
            {
                CannonSFX.clip = WeaponShootClip[3];

            }
        }
        else if (_WeaponType == LevelsData.WeaponType.Rock || _WeaponType == LevelsData.WeaponType.FireRock)
        {
           // Debug.Log("ROCKSENEMY");
            StartCoroutine(EnemyThrowRock());
            if (_WeaponType == LevelsData.WeaponType.Rock)
            {
                CannonSFX.clip = WeaponShootClip[0];

            }
            else if (_WeaponType == LevelsData.WeaponType.FireRock)
            {
                CannonSFX.clip = WeaponShootClip[1];
                CannonSFX.Play();
                //Debug.Log("FireRockFireRock");
            }
        }
        else if (_WeaponType == LevelsData.WeaponType.Cannon)
        {
           // Debug.Log("CannonSENEMY");
            StartCoroutine(EnemyShootCanon());
            if (_WeaponType == LevelsData.WeaponType.Cannon)
            {
                CannonSFX.clip = WeaponShootClip[4];

            }
        }
        yield return new WaitForSeconds(0.3f);
        GameObject Projectileshoot = Instantiate(GameManager.instance.levelsData[GameManager.instance.levelIndex].GoCastleAmmo, EnemyMuzzlePos[CannonIndex].position, EnemyMuzzlePos[CannonIndex].rotation);
            Projectileshoot.GetComponent<Rigidbody2D>().AddForce(transform.right * LaunchForce * -1);
        if (_WeaponType == LevelsData.WeaponType.FireArrow || _WeaponType == LevelsData.WeaponType.FireRock)
        {
            //Debug.Log("FIIRE");
            Projectileshoot.transform.GetChild(0).gameObject.SetActive(true);
            Projectileshoot. transform.GetChild(1).gameObject.SetActive(true);
        }
            Instantiate(DustSpawn, EnemyMuzzlePos[CannonIndex].position, EnemyMuzzlePos[CannonIndex].rotation);

        
        CannonSFX.Play();
        CannonIndex += 1;

        if(CannonIndex == 3)
            CannonIndex = 0;

    }
    public IEnumerator EnemyShootArrow()
    {
        CastleMachineryAnimator[CannonIndex].SetInteger("CastleMachinery", 1);
        int tempIndex = CannonIndex;
        yield return new WaitForSeconds(0.5f);
        CastleMachineryAnimator[tempIndex].SetInteger("CastleMachinery", 0);
    }
    public IEnumerator EnemyThrowRock()
    {
        //Debug.Log(CannonIndex + "CannonIndex");
        CastleMachineryAnimator[CannonIndex].SetInteger("CastleMachinery", 3);
        CastleMachineryAnimator[CannonIndex].SetInteger("CastleMachinery", 1);
        int tempIndex = CannonIndex;
        yield return new WaitForSeconds(0.15f);
        CastleMachineryAnimator[tempIndex].SetInteger("CastleMachinery", 2);
        yield return new WaitForSeconds(0.15f);
        CastleMachineryAnimator[tempIndex].SetInteger("CastleMachinery", 3);
        
    }
    public IEnumerator EnemyShootCanon()
    {

        CastleMachineryAnimator[CannonIndex].SetInteger("CastleMachinery", 1);

        int tempIndex = CannonIndex;
        yield return new WaitForSeconds(.5f);


        CastleMachineryAnimator[tempIndex].SetInteger("CastleMachinery", 0);
    }
    public void StopEnemyShooting()
    {
        CancelInvoke("InitiateShoot");
    }

    public void StartEnemyShooting()
    {
        InvokeRepeating("InitiateShoot", 2, 5);
    }
}
