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
     
    [HideInInspector]public string characterName;
    [HideInInspector]public int characterIndex = 0;
    
    int medievalCharMax = 2, modCharMax = 7, futCharMax = 11,mediLvlMax=5,modLvlMax=10,futLvlMax=15,startIndex=0,endIndex=0;
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    { 
        //CheckForCharacterUnlock();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("WeaponManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        characterIndex = PlayerPrefs.GetInt("Selected_Char");
    }

    #region --ButtonClick---
    public void ButtonClick_NextCharacter()
    {
        UIScript.instance.BT_Clik_Sound(true);
        if (characterIndex < endIndex)
            characterIndex++;
        else
            characterIndex = startIndex;

        SetCharacter();

    }
    public void ButtonClick_PreviousCharacter()
    {
        UIScript.instance.BT_Clik_Sound(false);
        if (characterIndex > startIndex)
            characterIndex--;
        else
            characterIndex = endIndex;

        SetCharacter();
    }
    #endregion

    #region --User Defined Methods--
    public void SetCharacter()
    {       // Set Machinery Data From Data Container
        if (PlayerPrefs.GetInt("Character" + characterIndex)==1)
        {
            characterName = lstCharactersData[characterIndex].CharacterName;
            if(imgChacterSprite)
                imgChacterSprite.sprite = lstCharactersData[characterIndex].SpriteCharacter;
            if(textCharacterName)
                textCharacterName.text = characterName;
            if(imgChacterSprite)
                imgChacterSprite.gameObject.GetComponent<Button>().interactable = true;
            if(UIScript.instance)
                UIScript.instance.PlayNow.interactable = true;
            PlayerPrefs.SetInt("Selected_Char",characterIndex);
        }
        else
        {
            characterName = lstCharactersData[characterIndex].CharacterName;
            imgChacterSprite.sprite = lstCharactersData[characterIndex].SpriteCharacter;
            textCharacterName.text = characterName;
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

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (PlayerPrefs.GetInt("Character"+i)==1)
                characterIndex = i;
        }
        SetCharacter();
    }

    void MachineryCrossCheck()
    {
        if(PlayerPrefs.GetInt("Machinery"+ MachineryManager.instance.machineryIndex) !=1)
        {
            UIScript.instance.PlayNow.interactable = false;
        }
    }
    #endregion
}
