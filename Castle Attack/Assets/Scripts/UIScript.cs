using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject MainUI, StoreUI, MachineryStoreUI, CharacterStoreUI, LevelUI, WeaponsUI;
    public GameObject Settings_SlidePanel;
    public Animation SettingsSlide;
    public Text InGameCoinsText;
    public static UIScript instance;
    public Button PlayNow;
    public Button btnPrevMachinery, btnNextMachinery, btnPrevCharacter, btnNextCharacter;
    private GameObject goMachineryManager;
    public Text textMahineName_TEMP, textCharacterName_TEMP;
    public Image imgMachinerySprite_TEMP, imgCharacterName_TEMP;
    public int SelectedProductIndex;
    public GameObject hideStoreParticles;

    [Header("[SETTINGS]")]
    public Image soundBtn;
    public Sprite[] soundSprites;
    int soundIndex;

    [Header("[STORE RESOURCES MACHINERY]")]
    public Image StoreMachineryImageSelect;
    public Transform storeMachineries;
    public Text StoreMachineryName;
    public Text StoreMachineryStrength;
    public Text StoreMachineryPower;
    public Text StoreMachineryCostValue;


    [Header("[STORE RESOURCES CHARACTER]")]
    public Image StoreCharacterImageSelect;
    public Transform storeCharaters;
    public Text StoreCharacterName;
    public Text StoreCharacterStrength;
    public Text StoreCharacterPower;
    public Text StoreCharacterCostValue;


    private void OnEnable()
    {
        btnPrevMachinery.onClick.AddListener(() => MachineryManager.instance.ButtonClick_PreviousMachine());
        btnNextMachinery.onClick.AddListener(() => MachineryManager.instance.ButtonClick_NextMachine());

        btnPrevCharacter.onClick.AddListener(() => CharacterManager.instance.ButtonClick_PreviousCharacter());
        btnNextCharacter.onClick.AddListener(() => CharacterManager.instance.ButtonClick_NextCharacter());
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        goMachineryManager = GameObject.Find("WeaponsManager");

        InGameCoinsText.text = "$" + PlayerPrefs.GetInt("InGameCoins").ToString();
        goMachineryManager.transform.GetComponent<MachineryManager>().textMahineName = textMahineName_TEMP;
        goMachineryManager.transform.GetComponent<MachineryManager>().imgMachinerySprite = imgMachinerySprite_TEMP;

        goMachineryManager.transform.GetComponent<CharacterManager>().textCharacterName = textCharacterName_TEMP;
        goMachineryManager.transform.GetComponent<CharacterManager>().imgChacterSprite = imgCharacterName_TEMP;
        goMachineryManager.transform.GetComponent<CharacterManager>().SetCharacter();

        //=== Sound
        soundIndex = PlayerPrefs.GetInt("Sound");
        AudioListener.volume = soundIndex;
        soundBtn.sprite = soundSprites[soundIndex];
    }

    public void ButtonClick_PlayMenu()
    {
        MainUI.SetActive(false);
        LevelUI.SetActive(true);
    }

    public void ButtonClick_Store(int no)
    {
        //===== New
        hideStoreParticles.SetActive(false);

        if (!StoreUI.activeInHierarchy)
            StoreUI.SetActive(true);
        // 0 - Machinery , 1 - Character
        if (no == 0)
        {
            if (CharacterStoreUI.activeInHierarchy)
                CharacterStoreUI.SetActive(false);

            MainUI.SetActive(false);
            MachineryStoreUI.SetActive(true);
            storeMachineries.gameObject.SetActive(true);
            OnStoreProductChange(0, 0);
        }
        else if (no == 1)
        {

            if (MachineryStoreUI.activeInHierarchy)
                MachineryStoreUI.SetActive(false);


            MainUI.SetActive(false);
            CharacterStoreUI.SetActive(true);
            storeCharaters.gameObject.SetActive(true);
            OnStoreProductChange(1, 0);
        }

    }

    public void OnBGButtonClick()
    {
        SwipeDetector.instance.currentEqId = 0;
        if (MachineryStoreUI.activeInHierarchy)
            MachineryStoreUI.SetActive(false);

        if (CharacterStoreUI.activeInHierarchy)
            CharacterStoreUI.SetActive(false);

        MainUI.SetActive(true);
        StoreUI.SetActive(false);

        //==== New
        hideStoreParticles.SetActive(true);
        storeMachineries.gameObject.SetActive(false);
        storeCharaters.gameObject.SetActive(false);
        //======    

    }

    public void ButtonClick_PlayWeapons()
    {
        SceneManager.LoadScene("GamePlayScene");
    }


    public void OnSettingsClick()
    {
        if (Settings_SlidePanel.activeInHierarchy)
        {
            SettingsSlide.Play("Settings_SLide_Back");
            Invoke("DisableSettings", 1);
            // Play Slide Back Animation;
            return;
        }


        Settings_SlidePanel.SetActive(true);
        SettingsSlide.Play("Settings_SLide");
    }

    void DisableSettings()
    {
        Settings_SlidePanel.SetActive(false);
    }

    public void OnStoreProductChange(int ProductType, int IndexNo)
    {
        //=== Machines
        if (ProductType == 0)
        {
            StoreMachineryName.text = MachineryManager.instance.lstMachineryData[IndexNo].MachineryName;

            //===== New
            for (int i = 0; i < storeMachineries.childCount; i++)
            {
                storeMachineries.GetChild(i).gameObject.SetActive(false);
                if (IndexNo == i)
                    storeMachineries.GetChild(i).gameObject.SetActive(true);
            }

            //=== Rock
            if (IndexNo == 0)
            {
                StartCoroutine(PerformActionForMachine(storeMachineries.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 0.15f, IndexNo));
            }
            else
            {
                StartCoroutine(PerformActionForMachine(storeMachineries.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 0.5f, IndexNo));
            }

            if (MachineryManager.instance.lstMachineryData[IndexNo].IsUnlock)
            {
                StoreMachineryCostValue.text = "Unlocked";
                StoreMachineryCostValue.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                StoreMachineryCostValue.text = MachineryManager.instance.lstMachineryData[IndexNo].IAPCost.ToString();
                StoreMachineryCostValue.transform.parent.GetComponent<Button>().interactable = true;
            }
        }
        //==== Characters
        else if (ProductType == 1)
        {
            StoreCharacterName.text = CharacterManager.instance.lstCharactersData[IndexNo].CharacterName;
            //IndexNo = 0;

            //===== New
            for (int i = 0; i < storeCharaters.childCount; i++)
            {
                storeCharaters.GetChild(i).gameObject.SetActive(false);
                if (IndexNo == i)
                    storeCharaters.GetChild(i).gameObject.SetActive(true);
            }

            if (IndexNo == 0)
            {
                StartCoroutine(PerformActionForCharacter(storeCharaters.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 1.3f, IndexNo));
            }
            else if (IndexNo == 1)
            {
                StartCoroutine(PerformActionForCharacter(storeCharaters.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 1.5f, IndexNo));
            }
            else if (IndexNo == 2)
            {
                StartCoroutine(PerformActionForCharacter(storeCharaters.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 1.7f, IndexNo));
            }


            if (CharacterManager.instance.lstCharactersData[IndexNo].IsUnlock)
            {
                StoreCharacterCostValue.text = "Unlocked";
                StoreCharacterCostValue.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                StoreCharacterCostValue.text = CharacterManager.instance.lstCharactersData[IndexNo].IAPCost.ToString();
                StoreCharacterCostValue.transform.parent.GetComponent<Button>().interactable = true;
            }
        }
    }

    private IEnumerator PerformActionForMachine(Animator animator, float delay, int index)
    {
        animator.SetInteger("Catapult", 1);
        yield return new WaitForSeconds(delay);

        //==== Rock
        if (index == 0 || index == 1)
        {
            animator.SetInteger("Catapult", 2);
            yield return new WaitForSeconds(delay);

            animator.SetInteger("Catapult", 3);
            yield return new WaitForSeconds(delay);
        }
        else
        {
            animator.SetInteger("Catapult", 0);
            yield return new WaitForSeconds(delay);
        }

        if (storeMachineries.gameObject.activeSelf)
            StartCoroutine(PerformActionForMachine(animator, delay, index));
        else
            StopCoroutine("PerformActionForMachine()");
    }

    private IEnumerator PerformActionForCharacter(Animator animator, float delay, int index)
    {
        animator.SetInteger("Player", 3);
        yield return new WaitForSeconds(delay / 4);

        animator.SetInteger("Player", 4);
        yield return new WaitForSeconds(delay);

        animator.SetInteger("Player", 5);
        yield return new WaitForSeconds(delay / 4);


        if (storeCharaters.gameObject.activeSelf)
            StartCoroutine(PerformActionForCharacter(animator, delay, index));
        else
            StopCoroutine("PerformActionForCharacter()");
    }


    public void OnStoreProductBuy()
    {
        int SelectedProductIndex = SwipeDetector.instance.currentEqId;
       
        if (MachineryStoreUI.activeInHierarchy)
        {
            if (PlayerPrefs.GetInt("Machinery" + SelectedProductIndex) != 1)
            {
                PlayerPrefs.SetInt("Machinery" + SelectedProductIndex, 1);
                MachineryManager.instance.lstMachineryData[SelectedProductIndex].IsUnlock = true;
                StoreMachineryCostValue.text = "Unlocked";
                StoreMachineryImageSelect.gameObject.GetComponent<Button>().interactable = false;
                StoreMachineryCostValue.transform.parent.GetComponent<Button>().interactable = false;
            }
        }
        else if (CharacterStoreUI.activeInHierarchy)
        {
            if (PlayerPrefs.GetInt("Character" + SelectedProductIndex) != 1)
            {
                PlayerPrefs.SetInt("Character" + SelectedProductIndex, 1);
                CharacterManager.instance.lstCharactersData[SelectedProductIndex].IsUnlock = true;
                StoreCharacterCostValue.text = "Unlocked";
                StoreCharacterImageSelect.gameObject.GetComponent<Button>().interactable = false;
                StoreCharacterCostValue.transform.parent.GetComponent<Button>().interactable = false;
            }
        }

    }

    public void BT_Sound_Click()
    {
        if (soundIndex == 0)
        {
            soundIndex = 1;
        }
        else
        {
            soundIndex = 0;
        }
        PlayerPrefs.SetInt("Sound", soundIndex);
        AudioListener.volume = soundIndex;
        soundBtn.sprite = soundSprites[soundIndex];
    }
}

