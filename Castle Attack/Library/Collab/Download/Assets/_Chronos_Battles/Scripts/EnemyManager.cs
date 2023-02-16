using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float LaunchForce = 300;
    public GameObject DustSpawn;
    public List<GameObject> EnemySpawn = new List<GameObject>();
    public LevelsData.WeaponType _WeaponType;

    [HideInInspector] public Animator[] CastleMachineryAnimator = new Animator[3];

    int RandomMiss;
    int CannonIndex;

    public static EnemyManager insance;

    private void Awake()
    {
        insance = this;
    }
    void Start()
    {
       StartEnemyShooting();
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
            int r = Random.Range(100, 400);
            LaunchForce = r;
        }
        else
        {
            int r;
            if(CannonIndex == 1)
            {
                r = Random.Range(180, 250);
                LaunchForce = r;
            }
            else
            {
                 r = Random.Range(250, 300);
                 LaunchForce = r;
            }
        }
        CharacterAttack.instance.enemyVoiceSource.Play();
        StartCoroutine(EnemyThrowBall(0.5f,1,0));
       
        yield return new WaitForSeconds(0.3f);
        GameObject Projectileshoot = Instantiate(GameManager.instance.levelsData[GameManager.instance.levelIndex].GoCastleAmmo, GameManager.instance.SpawnCastleGO.EnemyMuzzlePos[CannonIndex].position, GameManager.instance.SpawnCastleGO.EnemyMuzzlePos[CannonIndex].rotation);
        if (Projectileshoot.GetComponent<CannonHitInfo>())
            Projectileshoot.GetComponent<CannonHitInfo>().Shoot(LaunchForce);
        else
            Projectileshoot.transform.GetChild(0).GetComponent<CannonHitInfo>().Shoot(LaunchForce);

        if (_WeaponType == LevelsData.WeaponType.FireArrow || _WeaponType == LevelsData.WeaponType.FireRock)
        {
            Projectileshoot.transform.GetChild(0).gameObject.SetActive(true);
            Projectileshoot. transform.GetChild(1).gameObject.SetActive(true);
        }
        Instantiate(DustSpawn,GameManager.instance.SpawnCastleGO.EnemySpawnPos[CannonIndex].position, GameManager.instance.SpawnCastleGO.EnemySpawnPos[CannonIndex].rotation);

        if (Sound_Manager.instance)
            Sound_Manager.instance.enemyFire.Play();

        CannonIndex += 1;

        if(CannonIndex == 3)
            CannonIndex = 0;
    }

    public IEnumerator EnemyThrowBall(float delay,int firstIndex,int lastIndex)
    {
        CastleMachineryAnimator[CannonIndex].SetInteger("CastleMachinery", firstIndex);
        int tempIndex = CannonIndex;

        yield return new WaitForSeconds(delay);
        CastleMachineryAnimator[tempIndex].SetInteger("CastleMachinery", lastIndex);
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
