using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CastleLevel", menuName = "AddLevel")]
public class LevelsData : ScriptableObject
{
    [SerializeField]
    private int levelNo;
    public int LevelNo { get { return levelNo; } }

    public GameObject[] CastleMachineryPrefabs;

    [SerializeField] private int castleHP;
    public int CastleHP { get { return castleHP; } }

    [SerializeField] private int castleDamage;
    public int CastleDamage { get { return castleDamage; } }

    [SerializeField] private int castleReward;
    public int CastleReward { get { return castleReward; } }

    [SerializeField] private GameObject goCastleAmmo;
    public GameObject GoCastleAmmo { get { return goCastleAmmo; } }

    [SerializeField] public EraName era; // field
    public enum EraName { Medieval, Modern, Future }; // nested type

    [SerializeField] private GameObject[] goEnemies;
    public GameObject[] GoEnemies { get { return goEnemies; } }

    public enum WeaponType { Rock = 0, FireRock = 1, Arrow = 2, FireArrow = 3, Cannon = 4, M777A2=5, Panhard = 6, Rocket = 7, Tank = 8, Drone = 9, TwinTank=10, Robot=11, Helicopter=12, SpaceShip14 = 13, Spaceship15 = 14 };
    [SerializeField] private WeaponType weaponType;
    public WeaponType _WeaponType { get { return weaponType; } }

    public AudioClip fireClip, hitClip;

}
