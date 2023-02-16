using System;
using System.Collections;
using System.Collections.Generic;
using EasyMobile;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject MainUI, StoreUI, LevelUI, WeaponsUI, NotEnoughUI,BuyConfirmUI,QuitUI,StorePanel, LoadingUI,InfoUI;
    public Animation SettingsSlide;
    public Text InGameCoinsText;
    public static UIScript instance;
    public Button PlayNow,settingBtn;
    public Button btnPrevMachinery, btnNextMachinery, btnPrevCharacter, btnNextCharacter;
    private GameObject goMachineryManager;
    public Text textMahineName_TEMP, textCharacterName_TEMP;
    public Image imgMachinerySprite_TEMP, imgCharacterName_TEMP;
    public GameObject hideStoreParticles,Particles,CharacterImg;
    public GameObject swipeDetectedObj;

    [Header("[SETTINGS]")]
    public Image soundBtn;
    public Sprite[] soundSprites;
    int soundIndex;

    [Header("[STORE]")]
    public Transform storeMachineries;
    public Transform storeCharaters;
    public GameObject subStorePanel, subpowerPanel;
    public Text StoreItemName;
    public Text IapCostValue, IgcCostValue;
    public Text strongLevelText;
    public Transform[] storeBtns;
    public RectTransform iapBtn, igcBtn;
    public int productType = 0, SelectedProductIndex = 0;
    int coastType = 0;
    public Text powerTxt;
    public Button plusBtn;
    public Text buyMsg;
    public string timeInAppId, coinMulInAppId, coinsUpgInAppId;
    public Button[] plusBtns;
    public Text currBonTime, currBonMul;

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
        Advertising.Initialize();
        goMachineryManager = GameObject.Find("WeaponsManager");

        InGameCoinsText.text = PlayerPrefs.GetInt("InGameCoins").ToString();
        goMachineryManager.transform.GetComponent<MachineryManager>().textMahineName = textMahineName_TEMP;
        goMachineryManager.transform.GetComponent<MachineryManager>().imgMachinerySprite = imgMachinerySprite_TEMP;

        goMachineryManager.transform.GetComponent<CharacterManager>().textCharacterName = textCharacterName_TEMP;
        goMachineryManager.transform.GetComponent<CharacterManager>().imgChacterSprite = imgCharacterName_TEMP;
        goMachineryManager.transform.GetComponent<CharacterManager>().SetCharacter();

        //=== Sound
        soundIndex = PlayerPrefs.GetInt("Sound");
        AudioListener.volume = soundIndex;
        soundBtn.sprite = soundSprites[soundIndex];

        if(Sound_Manager.instance)
        {
            if(!Sound_Manager.instance.BgMusic.isPlaying)
            {
                Sound_Manager.instance.BgMusic.Play();
            }
        }    
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(MainUI.activeSelf)
            {
                MainUI.SetActive(false);
                QuitUI.SetActive(true);
            }
            else if(LevelUI.activeSelf)
            {
                LevelUI.SetActive(false);
                MainUI.SetActive(true);
            }
            else if(InfoUI.activeSelf)
            {
                InfoUI.SetActive(false);
                MainUI.SetActive(true);
            }
            else if (StoreUI.activeSelf && !NotEnoughUI.activeSelf && !BuyConfirmUI.activeSelf)
            {
                OnBGButtonClick();
            }
            else if (WeaponsUI.activeSelf)
            {
                WeaponsUI.SetActive(false);
                MainUI.SetActive(true);
            }
            else if(NotEnoughUI.activeSelf)
            {
                NotEnoughUI.SetActive(false);
            }
            else if(BuyConfirmUI.activeSelf)
            {
                BuyConfirmUI.SetActive(false);
            }
        }
    }

    public void ButtonClick_PlayMenu()
    {
        BT_Clik_Sound(true);
        CharacterManager.instance.ShowCharacterAsPerLevel((PlayerPrefs.GetInt("Lvl_Index")+1));
        MachineryManager.instance.ShowMachineAsPerLevel((PlayerPrefs.GetInt("Lvl_Index") + 1));
        MainUI.SetActive(false);
        WeaponsUI.SetActive(true);
    }

    public void ButtonClick_Store(int no)
    {
        BT_Clik_Sound(true);
        productType = no;
        hideStoreParticles.SetActive(false);
        CharacterImg.SetActive(false);
        Particles.SetActive(false);
        swipeDetectedObj.SetActive(true);
        StorePanel.SetActive(true);

        if (!StoreUI.activeInHierarchy)
        {
            StoreUI.SetActive(true);
            MainUI.SetActive(false);
        }

        if (productType == 3)//Timer
        {
            storeCharaters.gameObject.SetActive(false);
            storeMachineries.gameObject.SetActive(false);
            swipeDetectedObj.SetActive(false);
            OnStoreProductChange(0);
        }
        else
        { // 0 - Machinery , 1 - Character, 2 Power
            if (productType == 0 || productType == 2)
            {
                storeCharaters.gameObject.SetActive(false);
                storeMachineries.gameObject.SetActive(true);
            }
            else if (productType == 1)
            {
                storeMachineries.gameObject.SetActive(false);
                storeCharaters.gameObject.SetActive(true);
            }
            CheckEra(productType);
            swipeDetectedObj.SetActive(true);
        }
    }

    private void CheckEra(int no)
    {
        int lvlNo = 0;
        if (PlayerPrefs.GetInt("Level11") == 1)
            lvlNo = 11;
        else if (PlayerPrefs.GetInt("Level6") == 1)
            lvlNo = 6;
        else
            lvlNo = 1;

        if(no==0 || no==2)//machinery
            SwipeDetector.instance.ShowMachineAsPerEra(lvlNo,no);
        else//Character
            SwipeDetector.instance.ShowCharacterAsPerEra(lvlNo);
    }

    public void OnBGButtonClick()
    {
        BT_Clik_Sound(false);
        swipeDetectedObj.SetActive(false);
        SwipeDetector.instance.currentEqId = 0;
        StorePanel.SetActive(false);
        MainUI.SetActive(true);
        StoreUI.SetActive(false);

        hideStoreParticles.SetActive(true);
        Particles.SetActive(true);
        CharacterImg.SetActive(true);
        storeMachineries.gameObject.SetActive(false);
        storeCharaters.gameObject.SetActive(false);   
    }

    public void ButtonClick_PlayWeapons()
    {
        BT_Clik_Sound(true);
        PlayNow.interactable = false;
        WeaponsUI.SetActive(false);
        LoadingUI.SetActive(true);
        StartCoroutine(Loading("GamePlayScene"));
    }

    private IEnumerator Loading(string sceneName)
    {
        AsyncOperation operation= SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            yield return null;
        }
    }

    public void OnSettingsClick()
    {
        settingBtn.interactable = false;
        Invoke("DisableSettings", 0.8f);

        if (SettingsSlide.gameObject.activeInHierarchy)
        {
            SettingsSlide.Play("Settings_SLide_Back");
            BT_Clik_Sound(false);
            return;
        }
        BT_Clik_Sound(true);
        SettingsSlide.gameObject.SetActive(true);
        SettingsSlide.Play("Settings_SLide");
    }

    void DisableSettings()
    {
        if(SettingsSlide.transform.localScale!=new Vector3(1,1,1))
            SettingsSlide.gameObject.SetActive(false);
        settingBtn.interactable = true;
    }

    public void OnStoreProductChange(int IndexNo)
    {
        HighLightBtn();
        
        //=== Machines
        if (productType == 0 || productType==2)
        {
            subStorePanel.SetActive(true);
            subpowerPanel.SetActive(false);

            StoreItemName.text = MachineryManager.instance.lstMachineryData[IndexNo].MachineryName;
            iapBtn.anchoredPosition = new Vector2(0, iapBtn.anchoredPosition.y);
            igcBtn.gameObject.SetActive(false);
            powerTxt.gameObject.SetActive(true);
            strongLevelText.text = "STRONG AT LEVEL " + MachineryManager.instance.lstMachineryData[IndexNo].StrongLevel.ToString();

            //===== New
            for (int i = 0; i < storeMachineries.childCount; i++)
            {
                storeMachineries.GetChild(i).gameObject.SetActive(false);
                if (IndexNo == i)
                    storeMachineries.GetChild(i).gameObject.SetActive(true);
            }
            StartCoroutine(PerformActionForMachine(storeMachineries.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 0.5f, IndexNo));

            if (PlayerPrefs.GetInt("Machinery" + IndexNo) == 1)//Unlocked
            {
                setBtnsValue("Unlocked", false, "Unlocked", false);
                if (productType == 2)
                {
                    iapBtn.gameObject.SetActive(false);
                    plusBtn.gameObject.SetActive(true);
                    
                    if (PlayerPrefs.GetInt("Machinery" + IndexNo + "_Upgraded") == 1)//Upgraded
                    {
                        plusBtn.interactable = false;
                        if (PlayerPrefs.GetInt("Machinery" + IndexNo + "_Exhausted") == 1)
                            powerTxt.text = "7 "+ MachineryManager.instance.lstMachineryData[IndexNo].ammoType;
                        else
                            powerTxt.text = "4 " + MachineryManager.instance.lstMachineryData[IndexNo].ammoType;
                    }
                    else
                    {
                        plusBtn.interactable = true;
                        powerTxt.text = MachineryManager.instance.lstMachineryData[IndexNo].upgradationCoast + " COINS\n2 "
                    + MachineryManager.instance.lstMachineryData[IndexNo].ammoType;
                    }
                }
                else
                {
                    plusBtn.gameObject.SetActive(false);
                    iapBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                setBtnsValue(MachineryManager.instance.lstMachineryData[IndexNo].IAPCost.ToString(), true, "Unlocked", true);
                iapBtn.gameObject.SetActive(true);
                plusBtn.gameObject.SetActive(false);
                powerTxt.text = "SEVEN " + MachineryManager.instance.lstMachineryData[IndexNo].ammoType;
            }
        }
        //==== Characters
        else if (productType == 1)
        {
            subStorePanel.SetActive(true);
            subpowerPanel.SetActive(false);

            powerTxt.gameObject.SetActive(false);
            StoreItemName.text = CharacterManager.instance.lstCharactersData[IndexNo].CharacterName;
            iapBtn.gameObject.SetActive(true);
            iapBtn.anchoredPosition = new Vector2(-150, iapBtn.anchoredPosition.y);
            igcBtn.gameObject.SetActive(true);
            plusBtn.gameObject.SetActive(false);
            strongLevelText.text = "STRONG AT LEVEL " + CharacterManager.instance.lstCharactersData[IndexNo].StrongLevel.ToString();

            //===== New
            for (int i = 0; i < storeCharaters.childCount; i++)
            {
                storeCharaters.GetChild(i).gameObject.SetActive(false);
                if (IndexNo == i)
                    storeCharaters.GetChild(i).gameObject.SetActive(true);
            }

            if (IndexNo == 0)
                StartCoroutine(PerformActionForCharacter(storeCharaters.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 1.3f, IndexNo));
            else if (IndexNo == 1)
                StartCoroutine(PerformActionForCharacter(storeCharaters.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 1.5f, IndexNo));
            else if (IndexNo >= 2)
                StartCoroutine(PerformActionForCharacter(storeCharaters.GetChild(IndexNo).gameObject.GetComponent<Animator>(), 1.7f, IndexNo));

            if (PlayerPrefs.GetInt("Character"+IndexNo)!=0)
            {
                setBtnsValue("Unlocked", false, "Unlocked", false);
            }
            else
                setBtnsValue(CharacterManager.instance.lstCharactersData[IndexNo].IAPCost.ToString(), true,
                    CharacterManager.instance.lstCharactersData[IndexNo].IGCCost.ToString(), true);
        }
        else if(productType==3)
        {
            subStorePanel.SetActive(false);
            subpowerPanel.SetActive(true);

            if (PlayerPrefs.GetInt("Bonus_Time_No") == 2)
                plusBtns[0].interactable = false;

            if(PlayerPrefs.GetInt("Bonus_Multiplier_No") == 2)
                plusBtns[1].interactable = false;

            if (PlayerPrefs.GetInt("Coin_Upgrade_No") == 2)
                plusBtns[2].interactable = false;

            currBonTime.text = PlayerPrefs.GetFloat("Bonus_Time").ToString() + " Sec";
            currBonMul.text = "x" + PlayerPrefs.GetInt("Bonus_Multiplier");
        }
    }

    private void setBtnsValue(string s1, bool b1, string s2, bool b2)
    {
        IapCostValue.text = s1;
        IapCostValue.transform.parent.GetComponent<Button>().interactable = b1;

        IgcCostValue.text = s2;
        IgcCostValue.transform.parent.GetComponent<Button>().interactable = b2;
    }
        
    private void HighLightBtn()
    {
        for(int i=0;i<storeBtns.Length;i++)
        {
            if (i == productType)
                storeBtns[i].localScale = new Vector3(1.1f, 1.1f,1);
            else
                storeBtns[i].localScale = new Vector3(.8f, .8f, 1);
        }
    }

    private IEnumerator PerformActionForMachine(Animator animator, float delay, int index)
    {
        animator.SetInteger("Catapult", 1);
        yield return new WaitForSeconds(delay);

        animator.SetInteger("Catapult", 0);
        yield return new WaitForSeconds(delay);

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
        BT_Clik_Sound(true);
        if (SwipeDetector.instance)
            SelectedProductIndex = SwipeDetector.instance.currentEqId;

        if (productType==0 || productType==2)//Machinery || Power Upgrade
       {
            print("Machinery");
            if (PlayerPrefs.GetInt("Machinery" + SelectedProductIndex) != 1)//Not Unlocked
            {
                print("Not Unlocked");
                if (coastType == 0)//IAP
                {
                    print("IAP");
                   Store.instance.PurchaseSampleProduct(MachineryManager.instance.lstMachineryData[SelectedProductIndex].InAppId);
                }
            }
            else//Upgrade
            {
//                print("Upgarade Click");
                if (PlayerPrefs.GetInt("InGameCoins") >= MachineryManager.instance.lstMachineryData[SelectedProductIndex].upgradationCoast)
                {
                    PlayerPrefs.SetInt("InGameCoins", (int)(PlayerPrefs.GetInt("InGameCoins") - MachineryManager.instance.lstMachineryData[SelectedProductIndex].upgradationCoast));
                    InGameCoinsText.text = PlayerPrefs.GetInt("InGameCoins").ToString();
                    PlayerPrefs.SetInt("Machinery" + SelectedProductIndex+"_Upgraded", 1);
                    plusBtn.interactable = false;
                    powerTxt.text = "4 " + MachineryManager.instance.lstMachineryData[SelectedProductIndex].ammoType;
                }
                else
                {
                    NotEnoughUI.SetActive(true);
                }
            }
        }
        else if (productType==1)
        {
            print("Character");
            plusBtn.gameObject.SetActive(false);
            if (coastType == 0)//IAP
            {
                print("IAP");
                 Store.instance.PurchaseSampleProduct(CharacterManager.instance.lstCharactersData[SelectedProductIndex].InAppId);
            }
            else if (coastType == 1)//IGC
            {
                print("IGC");
                coastType = 0;
                if (PlayerPrefs.GetInt("InGameCoins") >= CharacterManager.instance.lstCharactersData[SelectedProductIndex].IGCCost)
                {
                    PlayerPrefs.SetInt("InGameCoins", (int)(PlayerPrefs.GetInt("InGameCoins") - CharacterManager.instance.lstCharactersData[SelectedProductIndex].IGCCost));
                    InGameCoinsText.text = PlayerPrefs.GetInt("InGameCoins").ToString();
                    Unlock_Product();
                }
                else
                {
                    NotEnoughUI.SetActive(true);
                }
            }
        }
        else if(productType==3)//Timer
        {
            Store.instance.PurchaseSampleProduct(timeInAppId);
        }
        else if (productType == 4)//Coin Multiplier
        {
            Store.instance.PurchaseSampleProduct(coinMulInAppId);
        }
        else if (productType == 5)//Free Coins
        {
            Store.instance.PurchaseSampleProduct(coinsUpgInAppId);
        }
    }

    public void Unlock_Product()
    {
        setBtnsValue("Unlocked", false, "Unlocked", false);
        if(productType==0)
        {
            print("Machine Unlock");
            PlayerPrefs.SetInt("Machinery" + SelectedProductIndex, 1);
        }
        else if (productType == 2)
        {
            print("Machine Unlock");
            PlayerPrefs.SetInt("Machinery" + SelectedProductIndex, 1);
            iapBtn.gameObject.SetActive(false);
            plusBtn.gameObject.SetActive(true);
            plusBtn.interactable = true;
            powerTxt.text = MachineryManager.instance.lstMachineryData[SelectedProductIndex].upgradationCoast + " COINS\n2 "
            + MachineryManager.instance.lstMachineryData[SelectedProductIndex].ammoType;
        }
        else if(productType==1)
        {
            print("Character Unlock");
            PlayerPrefs.SetInt("Character" + SelectedProductIndex, 1);
        }
        else if (productType == 3)//Timer
        {
            PlayerPrefs.SetFloat("Bonus_Time", PlayerPrefs.GetFloat("Bonus_Time")+ 15f);
            PlayerPrefs.SetInt("Bonus_Time_No", (PlayerPrefs.GetInt("Bonus_Time_No") + 1));

            currBonTime.text = PlayerPrefs.GetFloat("Bonus_Time").ToString() + " Sec";

            if (PlayerPrefs.GetInt("Bonus_Time_No") == 2)
                plusBtns[0].interactable = false;
        }
        else if (productType == 4)//Coin Multiplier
        {
            if((PlayerPrefs.GetInt("Bonus_Multiplier")==1))
                PlayerPrefs.SetInt("Bonus_Multiplier",5);
            else
                PlayerPrefs.SetInt("Bonus_Multiplier", 10);

            PlayerPrefs.SetInt("Bonus_Multiplier_No", (PlayerPrefs.GetInt("Bonus_Multiplier_No") + 1));

            currBonMul.text ="x"+ PlayerPrefs.GetInt("Bonus_Multiplier");

            if (PlayerPrefs.GetInt("Bonus_Multiplier_No") == 2)
                plusBtns[1].interactable = false;
        }
        else if (productType == 5)//Free Coins
        {
            
            PlayerPrefs.SetInt("Coin_Upgrade_No", (PlayerPrefs.GetInt("Coin_Upgrade_No")+1));
            PlayerPrefs.SetInt("InGameCoins", (PlayerPrefs.GetInt("InGameCoins") + 50000));
            InGameCoinsText.text = PlayerPrefs.GetInt("InGameCoins").ToString();

            if (PlayerPrefs.GetInt("Coin_Upgrade_No") == 2)
                plusBtns[2].interactable = false;
        }
    }

    public void BT_purchase_Click(int no)
    {
        BT_Clik_Sound(true);
       if(no==2)//Machinery Upgrade
            buyMsg.text = "Are you sure you want to upgrade this Item?";
       else// Machinery, Character, Power Up
            buyMsg.text = "Are you sure you want to purchase this Item?";

       if(EventSystem.current.currentSelectedGameObject.name== "IgcCoastBtn")
            coastType = 1;

        if (storeMachineries.gameObject.activeSelf)
            productType = 0;
        else if (storeMachineries.gameObject.activeSelf)
            productType = 1;
        else
            productType = no;

        BuyConfirmUI.SetActive(true);
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
        BT_Clik_Sound(true);
    }

    public void BT_Clik_Sound(bool positive)
    {
        if (Sound_Manager.instance)
        {
            if (positive)
            {
                Sound_Manager.instance.btnPositiveClick.Play();
            }
            else
            {
                Sound_Manager.instance.btnNegativeClick.Play();
            }
        }
    }
}

