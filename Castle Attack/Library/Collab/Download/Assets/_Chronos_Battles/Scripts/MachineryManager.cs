using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineryManager : MonoBehaviour
{
    public static MachineryManager instance;
    public Text textMahineName;
    public Image imgMachinerySprite;
    public List<MachinerysData> lstMachineryData;
    
    Sprite spriteMachinery;
    int medievalMacMax = 4, modMacMax = 9, futMacMax = 14, mediLvlMax = 5, modLvlMax = 10, futLvlMax = 15, startIndex = 0, endIndex = 0;

     public int selMachineryNo, machineryDamage, machineryHP, machineryIndex = 0;
    [HideInInspector] public string machineryName;
    [HideInInspector] public GameObject goMachineryAmmo, goMachineryPrefab;
    [HideInInspector]public MachinerysData.WeaponType _WeaponType;
    public MachinerysData data;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckForMachineryUnlock();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("WeaponManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        machineryIndex = PlayerPrefs.GetInt("Selected_Mac");
    }

    #region --ButtonClick---
    public void ButtonClick_NextMachine()
    {
        UIScript.instance.BT_Clik_Sound(true);
        if (machineryIndex < endIndex)
            machineryIndex++;
        else
            machineryIndex = startIndex;

        SetMachine();

    }
    public void ButtonClick_PreviousMachine()
    {
        UIScript.instance.BT_Clik_Sound(false);
        if (machineryIndex > startIndex)
            machineryIndex--;
        else
            machineryIndex = endIndex;

        SetMachine();
    }
    #endregion

    #region --User Defined Methods--
    public void SetMachine()
    {
        selMachineryNo = lstMachineryData[machineryIndex].MachineryNo;
        machineryDamage = lstMachineryData[machineryIndex].Damage;
        machineryHP = lstMachineryData[machineryIndex].HP;
        machineryName = lstMachineryData[machineryIndex].MachineryName;
        goMachineryAmmo = lstMachineryData[machineryIndex].GoAmmo;
        spriteMachinery = lstMachineryData[machineryIndex].SpriteMachinery;

        if(lstMachineryData[machineryIndex].MachineryPrefab)
            goMachineryPrefab = lstMachineryData[machineryIndex].MachineryPrefab;
        if(imgMachinerySprite)
            imgMachinerySprite.sprite = spriteMachinery;
        if(textMahineName)
            textMahineName.text = machineryName;
        _WeaponType = lstMachineryData[machineryIndex]._WeaponType;

        if (PlayerPrefs.GetInt("Machinery" + machineryIndex)==1)
        {
            if (imgMachinerySprite)
                imgMachinerySprite.gameObject.GetComponent<Button>().interactable = true;
            if(UIScript.instance)
                UIScript.instance.PlayNow.interactable = true;
            PlayerPrefs.SetInt("Selected_Mac", machineryIndex);
        }
        else
        {
            imgMachinerySprite.gameObject.GetComponent<Button>().interactable = false;
            UIScript.instance.PlayNow.interactable = false;
        }
        CharacterCrossCheck();
    }

    public void ShowMachineAsPerLevel(int lvlNo)
    {
        //Medilevel
        if (lvlNo <= mediLvlMax)
        {
            startIndex = 0;
            endIndex = medievalMacMax;
        }
        //Modern
        else if (lvlNo <= modLvlMax)
        {
            startIndex = medievalMacMax + 1;
            endIndex = modMacMax;
        }
        //Future
        else if (lvlNo <= futLvlMax)
        {
            startIndex = modMacMax + 1;
            endIndex = futMacMax;
        }

        for(int i=startIndex;i<=endIndex;i++)
        {
            if(PlayerPrefs.GetInt("Machinery" +i)==1)
                machineryIndex = i;
        }
        SetMachine();
    }

    void CheckForMachineryUnlock()
    {
        for (int i = 0; i < lstMachineryData.Count; i++)
        {
            if (PlayerPrefs.GetInt("Machinery" + i) == 1)
            {
                PlayerPrefs.SetInt("Machinery" + machineryIndex, 1);
            }
        }
    }

     void CharacterCrossCheck()
    {
        if (PlayerPrefs.GetInt("Character" + CharacterManager.instance.characterIndex) != 1)
        {
            UIScript.instance.PlayNow.interactable = false;
        }
    } 
    #endregion
}
