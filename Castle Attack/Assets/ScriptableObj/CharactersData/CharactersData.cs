using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "AddCharacter", order = 1)]
public class CharactersData : ScriptableObject
{

    [SerializeField] private int characterNo;
    public int CharacterNo { get { return characterNo; } }

    [SerializeField] private string characterName;
    public string CharacterName { get { return characterName; } }

    [SerializeField] private string iapCost;
    public string IAPCost { get { return iapCost; } }

    [SerializeField] private int igcCost;
    public int IGCCost { get { return igcCost; } }

    [SerializeField] private int bonusRewards;
    public int BonusRewards { get { return bonusRewards; } }

    [SerializeField] private Sprite spriteCharacter;
    public Sprite SpriteCharacter { get { return spriteCharacter; } }

    [SerializeField] private Animator animatorCombat;
    public Animator AnimatorCombat { get { return animatorCombat; } }

    [SerializeField] private Animator animatorGamePlay;
    public Animator AnimatorGamePlay { get { return animatorGamePlay; } }

    [SerializeField] private Animator animatorStore;
    public Animator AnimatorStore { get { return animatorStore; } }

    [SerializeField] private GameObject characterPrefab;
    public GameObject CharacterPrefab { get { return characterPrefab; } }

    [SerializeField] public EraName era; // field
    public enum EraName { Medieval, Modern, Future }; // nested type

    [SerializeField] private bool isUnlock;
    public bool IsUnlock { get { return isUnlock; } set { isUnlock = value; } }
}
