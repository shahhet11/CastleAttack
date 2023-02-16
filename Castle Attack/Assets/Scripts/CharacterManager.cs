using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public Text textCharacterName;
    public Image imgChacterSprite;

    // Character Data Variables...
    public List<CharactersData> lstCharactersData;
    public int selCharacterNo;
    public int iapCost;
    public int igcCost;
    public string characterName;
    public Sprite characterSprite;
    public GameObject goCharacterPrefab;
    public int characterIndex = 0;
    int totalCharacter = 0;
    int medievalCharMax = 2, modCharMax = 7, futCharMax = 11,mediLvlMax=5,modLvlMax=10,futLvlMax=15,startIndex=0,endIndex=0;
    


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    { 
        CheckForCharacterUnlock();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("WeaponManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        totalCharacter = lstCharactersData.Count;
        characterIndex = PlayerPrefs.GetInt("Selected_Char");
        //CharacterManager.instance.ShowCharacterAsPerLevel(lvlNo);
        //SetCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region --ButtonClick---
    public void ButtonClick_NextCharacter()
    {
        /*if (characterIndex < totalCharacter - 1)
            characterIndex++;
        else
            characterIndex = 0;*/
        if (characterIndex < endIndex)
            characterIndex++;
        else
            characterIndex = startIndex;

        SetCharacter();

    }
    public void ButtonClick_PreviousCharacter()
    {
       /* if (characterIndex > 0)
            characterIndex--;
        else
            characterIndex = totalCharacter - 1;*/

        if (characterIndex > startIndex)
            characterIndex--;
        else
            characterIndex = endIndex;


        SetCharacter();
    }
    #endregion

    #region --User Defined Methods--
    public void SetCharacter()
    {
        // Set Machinery Data From Data Container
        if (lstCharactersData[characterIndex].IsUnlock)
        {
            selCharacterNo = lstCharactersData[characterIndex].CharacterNo;
            characterName = lstCharactersData[characterIndex].CharacterName;
            characterSprite = lstCharactersData[characterIndex].SpriteCharacter;

            imgChacterSprite.sprite = lstCharactersData[characterIndex].SpriteCharacter;
            textCharacterName.text = characterName;
            goCharacterPrefab = lstCharactersData[characterIndex].CharacterPrefab;
            imgChacterSprite.gameObject.GetComponent<Button>().interactable = true;
            UIScript.instance.PlayNow.interactable = true;
            PlayerPrefs.SetInt("Selected_Char",characterIndex);
        }
        else {
            selCharacterNo = lstCharactersData[characterIndex].CharacterNo;
            characterName = lstCharactersData[characterIndex].CharacterName;
            characterSprite = lstCharactersData[characterIndex].SpriteCharacter;

            imgChacterSprite.sprite = lstCharactersData[characterIndex].SpriteCharacter;
            textCharacterName.text = characterName;
            goCharacterPrefab = lstCharactersData[characterIndex].CharacterPrefab;
            imgChacterSprite.gameObject.GetComponent<Button>().interactable = false;
            UIScript.instance.PlayNow.interactable = false;

        }
        MachineryCrossCheck();
    }

    public void ShowCharacterAsPerLevel(int lvlNo)
    {
        //Medilevel
        if (lvlNo <= mediLvlMax)
        {
            startIndex = 0;
            endIndex = medievalCharMax;
        }
        //Modern
        else if (lvlNo <= modLvlMax)
        {
            startIndex = medievalCharMax+1;
            endIndex = modCharMax;
        }
        //Future
        else if(lvlNo<=futLvlMax)
        {
            startIndex = modCharMax+1;
            endIndex = futCharMax;
        }
        
        if (characterIndex < startIndex || characterIndex > endIndex)
        {
            characterIndex = startIndex;
        }
        SetCharacter();
    }

    void CheckForCharacterUnlock()
    {
        for (int i = 0; i < lstCharactersData.Count; i++)
        {
            if(PlayerPrefs.GetInt("Character"+i) ==  1)
            {
                lstCharactersData[i].IsUnlock = true;
            }
        }
    }

    void MachineryCrossCheck()
    {
        if (!MachineryManager.instance.lstMachineryData[MachineryManager.instance.machineryIndex].IsUnlock)
        {
            UIScript.instance.PlayNow.interactable = false;
        }
    }
    #endregion
}
