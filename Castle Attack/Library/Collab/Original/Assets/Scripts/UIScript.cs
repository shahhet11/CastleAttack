using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject MainUI, StoreUI, LevelUI, WeaponsUI;

    public static UIScript instance;

    public Button btnPrevMachinery, btnNextMachinery;
    public Button btnPrevCharacter, btnNextCharacter;
    private GameObject WeaponManagerGo;
    public Text textMahineName_TEMP;
    public Image imgMachinerySprite_TEMP;
    
    public Text textPlayerName_TEMP;
    public Image imgPlayerSprite_TEMP;

    private void OnEnable()
    {
        btnPrevMachinery.onClick.AddListener(() => MachineryManager.instance.ButtonClick_PreviousMachine());
        btnNextMachinery.onClick.AddListener(() => MachineryManager.instance.ButtonClick_NextMachine());
        btnPrevCharacter.onClick.AddListener(() => CharacterManager.instance.ButtonClick_PreviousCharacter());
        btnNextCharacter.onClick.AddListener(() => CharacterManager.instance.ButtonClick_NextCharacter());

    }

    void Start()
    {
        if (instance == null)
            instance = this;


        WeaponManagerGo = GameObject.Find("WeaponsManager");

        WeaponManagerGo.transform.GetComponent<MachineryManager>().textMahineName = textMahineName_TEMP;
        WeaponManagerGo.transform.GetComponent<MachineryManager>().imgMachinerySprite = imgMachinerySprite_TEMP;

        

        WeaponManagerGo.transform.GetComponent<CharacterManager>().textCharacterName = textPlayerName_TEMP;
        WeaponManagerGo.transform.GetComponent<CharacterManager>().imgChacterSprite = imgPlayerSprite_TEMP;

    }

    public void ButtonClick_PlayMenu()
    {
        MainUI.SetActive(false);
        LevelUI.SetActive(true);
    }

    public void ButtonClick_Store()
    {
        MainUI.SetActive(false);
        StoreUI.SetActive(true);
    }

    public void OnBGButtonClick()
    {
        MainUI.SetActive(true);
        StoreUI.SetActive(false);
    }

    public void ButtonClick_PlayWeapons()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
}

