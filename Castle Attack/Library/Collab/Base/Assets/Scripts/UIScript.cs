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
    private GameObject WeaponManagerGo;
    public Text textMahineName_TEMP;
    public Image imgMachinerySprite_TEMP;
     
    private void OnEnable()
    {
        btnPrevMachinery.onClick.AddListener(() => MachineryManager.instance.ButtonClick_PreviousMachine());
        btnNextMachinery.onClick.AddListener(() => MachineryManager.instance.ButtonClick_NextMachine());
    }

    void Start()
    {
        if (instance == null)
            instance = this;


        WeaponManagerGo = GameObject.Find("WeaponsManager");

        WeaponManagerGo.transform.GetComponent<MachineryManager>().textMahineName = textMahineName_TEMP;
        WeaponManagerGo.transform.GetComponent<MachineryManager>().imgMachinerySprite = imgMachinerySprite_TEMP;

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

