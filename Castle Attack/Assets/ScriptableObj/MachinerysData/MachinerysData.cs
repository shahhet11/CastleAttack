using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineryData", menuName = "AddMachinery", order = 1)]
public class MachinerysData : ScriptableObject
{

    [SerializeField] private int machineryNo;
    public int MachineryNo { get { return machineryNo; } }

    [SerializeField] private string machineryName;
    public string MachineryName { get { return machineryName; } }

    [SerializeField] private string iapCost;
    public string IAPCost { get { return iapCost; } }

    [SerializeField] private float igcCost;
    public float IGCCost { get { return igcCost; } }

    [SerializeField] private int hp;
    public int HP { get { return hp; } }

    [SerializeField] private int ammo;
    public int Ammo { get { return ammo; } }

    [SerializeField] private int damage;
    public int Damage { get { return damage; } }

    [SerializeField] private Sprite spriteMachinery;
    public Sprite SpriteMachinery { get { return spriteMachinery; } }

    [SerializeField] private GameObject machineryPrefab;
    public GameObject MachineryPrefab { get { return machineryPrefab; } }

    [SerializeField] private Animator animatorMachinery;
    public Animator AnimatorMachinery { get { return animatorMachinery; } }

    [SerializeField] private Animation animationEra;
    public Animation AnimationEra { get { return animationEra; } }

    [SerializeField] private bool isUnlock;
    public bool IsUnlock { get { return isUnlock; } set { isUnlock = value; } }

    [SerializeField] public EraName era; // field
    public enum EraName { Medieval, Modern, Future }; // nested type

    [SerializeField] private GameObject goAmmo;
    public GameObject GoAmmo { get { return goAmmo; } }

    public enum WeaponType { Rock = 0, FireRock = 1, Arrow = 2, FireArrow = 3, Cannon = 4,M777A2=5};
    [SerializeField] private WeaponType weaponType;
    public WeaponType _WeaponType { get { return weaponType; } }
}
