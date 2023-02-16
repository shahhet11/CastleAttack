using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CastleLevel", menuName = "AddLevel")]
public class LevelsData : ScriptableObject
{
    [SerializeField]
    private int levelNo;
    public int LevelNo { get { return levelNo; } }

    [SerializeField] private Sprite spriteCastle;
    public Sprite SpriteCastle { get { return spriteCastle; } }

    [SerializeField] private Sprite spriteCastleMachinery;
    public Sprite SpriteCastleMachinery { get { return spriteCastleMachinery; } }

    [SerializeField] private GameObject castleMachineryPrefab;
    public GameObject CastleMachineryPrefab { get { return castleMachineryPrefab; } }

    [SerializeField] private int castleHP;
    public int CastleHP { get { return castleHP; } }

    [SerializeField] private int castleDamage;
    public int CastleDamage { get { return castleDamage; } }

    [SerializeField] private int castleReward;
    public int CastleReward { get { return castleReward; } }

    [SerializeField] private GameObject goCastleAmmo;
    public GameObject GoCastleAmmo { get { return goCastleAmmo; } }

    [SerializeField] private Animator castleAnimator;
    public Animator CastleAnimator { get { return castleAnimator; } }

    [SerializeField] private Animator spriteCastleMachineryAnimator;
    public Animator SpriteCastleMachineAnimator { get { return spriteCastleMachineryAnimator; } }

    [SerializeField] private int castleMachinery;
    public int CastleMachinery { get { return castleMachinery; } }

    [SerializeField] private bool isLock;
    public bool IsLock { get { return isLock; } }

    [SerializeField] public EraName era; // field
    public enum EraName { Medieval, Modern, Future }; // nested type

    [SerializeField] private GameObject goCastle;
    public GameObject GoCastle { get { return goCastle; } }

    [SerializeField] private GameObject[] goEnemies;
    public GameObject[] GoEnemies { get { return goEnemies; } }

    public enum WeaponType { Rock = 0, FireRock = 1, Arrow = 2, FireArrow = 3, Cannon = 4 };
    [SerializeField] private WeaponType weaponType;
    public WeaponType _WeaponType { get { return weaponType; } }
}
