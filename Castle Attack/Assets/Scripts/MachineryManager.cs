using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineryManager : MonoBehaviour
{
    public static MachineryManager instance;

    public Text textMahineName;
    public Image imgMachinerySprite;

    // Machinery Data Variables...
    public Transform trLevelMachineryParent;
    public List<MachinerysData> lstMachineryData;
    public int selMachineryNo;
    public int machineryDamage;
    public int machineryHP;
    public string machineryName;
    public GameObject goMachineryAmmo;
    public Sprite spriteMachinery;
    public GameObject goMachineryPrefab;
    public int machineryIndex = 0;
    int totalMachinery = 0;
    int medievalMacMax = 4, modMacMax = 9, futMacMax = 14, mediLvlMax = 5, modLvlMax = 10, futLvlMax = 15, startIndex = 0, endIndex = 0;


    public MachinerysData.WeaponType _WeaponType;

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

        totalMachinery = lstMachineryData.Count;
        machineryIndex = PlayerPrefs.GetInt("Selected_Mac");
        // SetMachine();
    }



    // Update is called once per frame
    void Update()
    {

    }


    #region --ButtonClick---
    public void ButtonClick_NextMachine()
    {
        /*if (machineryIndex < totalMachinery - 1)
            machineryIndex++;
        else
            machineryIndex = 0;*/

        if (machineryIndex < endIndex)
            machineryIndex++;
        else
            machineryIndex = startIndex;

        SetMachine();

    }
    public void ButtonClick_PreviousMachine()
    {
        /*if (machineryIndex > 0)
            machineryIndex--;
        else
            machineryIndex = totalMachinery - 1;*/

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
        // Set Machinery Data From Data Container
        if (lstMachineryData[machineryIndex].IsUnlock)
        {
            selMachineryNo = lstMachineryData[machineryIndex].MachineryNo;
            machineryDamage = lstMachineryData[machineryIndex].Damage;
            machineryHP = lstMachineryData[machineryIndex].HP;
            machineryName = lstMachineryData[machineryIndex].MachineryName;
            goMachineryAmmo = lstMachineryData[machineryIndex].GoAmmo;
            spriteMachinery = lstMachineryData[machineryIndex].SpriteMachinery;
            goMachineryPrefab = lstMachineryData[machineryIndex].MachineryPrefab;
            imgMachinerySprite.sprite = spriteMachinery;
            textMahineName.text = machineryName;
            _WeaponType = lstMachineryData[machineryIndex]._WeaponType;
            imgMachinerySprite.gameObject.GetComponent<Button>().interactable = true;
            UIScript.instance.PlayNow.interactable = true;
            PlayerPrefs.SetInt("Selected_Mac", machineryIndex);
        }

        else {

            selMachineryNo = lstMachineryData[machineryIndex].MachineryNo;
            machineryDamage = lstMachineryData[machineryIndex].Damage;
            machineryHP = lstMachineryData[machineryIndex].HP;
            machineryName = lstMachineryData[machineryIndex].MachineryName;
            goMachineryAmmo = lstMachineryData[machineryIndex].GoAmmo;
            spriteMachinery = lstMachineryData[machineryIndex].SpriteMachinery;
            goMachineryPrefab = lstMachineryData[machineryIndex].MachineryPrefab;
            imgMachinerySprite.sprite = spriteMachinery;
            textMahineName.text = machineryName;
            _WeaponType = lstMachineryData[machineryIndex]._WeaponType;
            imgMachinerySprite.gameObject.GetComponent<Button>().interactable = false;
            UIScript.instance.PlayNow.interactable = false;

        }
        CharacterCrossCheck();
        //selMachineryNo = machineryIndex;
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
       
        if (machineryIndex < startIndex || machineryIndex > endIndex)
        {
            machineryIndex = startIndex;
        }
        SetMachine();
    }

    void CheckForMachineryUnlock()
    {
        for (int i = 0; i < lstMachineryData.Count; i++)
        {
            if (PlayerPrefs.GetInt("Machinery" + i) == 1)
            {
                lstMachineryData[i].IsUnlock = true;
            }
        }
    }

     void CharacterCrossCheck()
    {
        if (!CharacterManager.instance.lstCharactersData[CharacterManager.instance.characterIndex].IsUnlock)
        {
            UIScript.instance.PlayNow.interactable = false;
        }
    } 
    #endregion
}
