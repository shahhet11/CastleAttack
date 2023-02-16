using System.Collections;
using System.Collections.Generic;
using ChartboostSDK;
using DG.Tweening;
using EasyMobile;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region -- OpVars--
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pool> pools;
    Dictionary<string, Queue<GameObject>> poolDictionary;
    [HideInInspector]public bool fingertouch = false, IsDustParticles = false;
    public GameObject[] Flame;
    public GameObject Player;

    public GameObject[] BlastPrefab;
    public GameObject BlastPrefabCannon;
    public GameObject BloodParticle;
    public GameObject ThrowableObject;
    public Transform[] ThrowableSpawnPosition;
    public bool isPaused, SpawnNew;
    public GameObject PauseText;
    public GameObject PauseHomeBtn;
    public GameObject[] dots;

    public static GameManager instance;
    public Animation AmmoCounter;
    [HideInInspector]public int TotalAmmo = 11, PlayerScore = 0,currentCastleHP = 0, currentMachineryHP = 0, ThisMachineryHP = 0;
    public Text AmmoText;
    public GameObject[] ammoImg;
    public GameObject Result;
    public Image ResultStatusIcon;
    public Sprite[] ResultImages;
    public Image CastleHealthFillbar;
     
    public Image CatapultHealthFillbar;
    public DOTweenAnimation fillBar, aimingRangeBtn;
    public bool GameOver;
    public Text ScoreText;
    public GameObject TrajectoryDots;
    public GameObject InGameHomeMenu;
   
    [Header("=====[MACHINERY DATA VAR]=====")]
    public List<MachinerysData> machinerysDatas;
    public GameObject MachinerySpawnPos1, MachinerySpawnPos2;
    int MachineryIndex;

    // Level Data Varaiables...
    [Header("=====[LEVEL DATA VAR]=====")]
    public SpawnPositions[] environments;
    public Transform trLevelCastleParent;
    public Text textLevelNo, textCastleData;
    public GameObject VictoryText;
    public List<LevelsData> levelsData;
    int selLevelNo;
    [HideInInspector]public int levelCastleHP,levelCastleDamage, levelIndex = 0;

   

    public SpawnPositions SpawnCastleGO;
    bool isDefeat;

    [Header("=====[Player DATA VAR]=====")]
    public List<CharactersData> characterData;
    public Transform trPlayerCharacterParent;
    GameObject goLevelCharacter;
    public GameObject playerMachine;
    int characterIndex = 0;
    public GameObject CharacterSpawnPos;
    public GameObject storeChar;
    public Button AttackBtnClick;
    public Text buttonText;

    [Header("=====[GAME OVER]=====")]
    public Image soundBtn;
    public Sprite[] soundSprites;
    int soundIndex;
    public Text coordsTExt;

    [Header("=====[BONUS ROUND]=====")]
    public GameObject winParticle;
    public GameObject AttackBtn;
    float Timer = 15;
    public int coinMul = 1;
    bool isBonusRound;
    public Transform EnempySpawnPos;
    [HideInInspector]public bool CHK_GAME_WIN = false,isGameStarted=false;
    private bool isAd = false;
    public string strAdHide;
    public Button hideBtn;

    #endregion

    #region -- Unity Default Methods--

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        levelIndex=PlayerPrefs.GetInt("Lvl_Index");
        Timer= PlayerPrefs.GetFloat("Bonus_Time");
        coinMul= PlayerPrefs.GetInt("Bonus_Multiplier");

        MachineryIndex = MachineryManager.instance.machineryIndex;
        characterIndex = CharacterManager.instance.characterIndex;
        selLevelNo = levelsData[levelIndex].LevelNo;
        levelCastleDamage = levelsData[levelIndex].CastleDamage;
        levelCastleHP = levelsData[levelIndex].CastleHP;
        goLevelCharacter = characterData[characterIndex].CharacterPrefab;
        storeChar = Instantiate(goLevelCharacter, environments[levelIndex].charPos.position, Quaternion.identity);
        storeChar.transform.SetParent(trPlayerCharacterParent); 
        EnemyManager.insance._WeaponType = levelsData[levelIndex]._WeaponType;
        
        SetLevel();
        SetMachinery();
        SpawnThrowableItems();

        AttackBtnClick.onClick.AddListener(() => CharacterAttack.instance.OnAttackButtonPressed());
        AttackBtn.SetActive(false);

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectpool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectpool);
        }
        InvokeRepeating("FingerTouchParticles", 1f, 0.1f);

        //=== Sound
        soundIndex = PlayerPrefs.GetInt("Sound");
        AudioListener.volume = soundIndex;
        soundBtn.sprite = soundSprites[soundIndex];
        if (PlayerPrefs.GetInt("HideAd") != 1)
            Chartboost.cacheInterstitial(CBLocation.Default);
       
        if (Sound_Manager.instance)
        {
            if (!Sound_Manager.instance.BgMusic.isPlaying)
            {
                Sound_Manager.instance.BgMusic.Play();
            }
            Sound_Manager.instance.CheckMachinery();
        }
        if (PlayerPrefs.GetInt("HideAd") != 1)
            Advertising.ShowBannerAd(BannerAdPosition.Bottom);
        else
            hideBtn.interactable = false;
    }

    IEnumerator CheckForPlayerNullInit(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(Player == null)
        {
            if (SpawnNew == false)
            {
                SpawnNew = true;
            }
        }
    }

    public void OnNextButtonClick()
    {
        BT_Clik_Sound(true);
        Advertising.HideBannerAd();
        if (Sound_Manager.instance)
        {
            Sound_Manager.instance.defeat.Stop();
            Sound_Manager.instance.victory.Stop();
        }
        if (CHK_GAME_WIN && levelIndex==0)//All Levels Completed
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (SpawnNew && !GameOver)
        {
            InitiateNewSpawn();
            SpawnNew = false;
            AmmoCounter.Play();
        }

        if(isBonusRound)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                // Bonus Round Over
                isBonusRound = false;
                Timer = 15f;
                CancelInvoke("Call_Spawn_Enemy");
                
                Invoke("CallFinalVictoryScreen",4);
            }
        }
    }

    #endregion

    #region -- User Defined Methods--

    void SetLevel()
    {
        for (int i = 0; i < levelsData[levelIndex].GoEnemies.Length; i++)
        {
            EnemyManager.insance.EnemySpawn.Add(levelsData[levelIndex].GoEnemies[i]);
        }
        TotalAmmo = machinerysDatas[levelIndex].Ammo;
        
        AmmoText.text = TotalAmmo.ToString();
        currentMachineryHP = machinerysDatas[MachineryIndex].HP;
        ThisMachineryHP = currentMachineryHP;
        textLevelNo.text = "Level " + selLevelNo +" \nMachinery \n\t"+ currentMachineryHP +"-"+MachineryManager.instance.machineryDamage +"\n\t"+MachineryManager.instance.machineryName +"\n Player \n\t"+CharacterManager.instance.characterName;

        SpawnCastleGO = environments[levelIndex];
        SpawnCastleGO.gameObject.SetActive(true);
        EnempySpawnPos = environments[levelIndex].AiEnemySpawnPos;

        for(int i=0;i<environments[levelIndex].Flame.Length;i++)
        {
            Flame[i] = environments[levelIndex].Flame[i];
        }
        if (levelIndex == 7)
        {
            for (int i = 0; i < TrajectoryDots.transform.childCount; i++)
            {
                TrajectoryDots.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        for (int i = 0; i < SpawnCastleGO.EnemySpawnPos.Length; i++)
        {
            GameObject Go =  Instantiate(levelsData[levelIndex].CastleMachineryPrefabs[i], SpawnCastleGO.EnemySpawnPos[i].position, Quaternion.identity);
            EnemyManager.insance.CastleMachineryAnimator[i] = Go.transform.Find("_Animator").GetComponent<Animator>();
            Go.transform.SetParent(SpawnCastleGO.transform);
        }
         
        currentCastleHP = levelCastleHP;
        
        CastleHealthFillbar.fillAmount = 1;
        CatapultHealthFillbar.fillAmount = 1;
        textCastleData.text = "HP: " + currentCastleHP + " - " + levelCastleDamage;
    }

    void SetMachinery()
    {
        currentMachineryHP = MachineryManager.instance.machineryHP;
        ThrowableObject = MachineryManager.instance.goMachineryAmmo;
        ammoImg[(MachineryManager.instance.selMachineryNo - 1)].SetActive(true);

        if((MachineryManager.instance.selMachineryNo - 1) ==9 || (MachineryManager.instance.selMachineryNo - 1) >= 12)
            playerMachine = Instantiate(MachineryManager.instance.goMachineryPrefab, MachinerySpawnPos2.transform.position, Quaternion.identity);
        else
            playerMachine = Instantiate(MachineryManager.instance.goMachineryPrefab, MachinerySpawnPos1.transform.position, Quaternion.identity);
    }

    void FingerTouchParticles()
    {
        if (fingertouch != false)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                SpawnFromPool("FingerParticle", hit.point, Quaternion.identity);
            }
        }
    }

    public void OnHomeButtonClick()
    {
        BT_Clik_Sound(false);
        Advertising.HideBannerAd();
        if (Sound_Manager.instance)
        {
            Sound_Manager.instance.defeat.Stop();
            Sound_Manager.instance.victory.Stop();
        }
        if (Time.timeScale != 1)
            Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OnPauseButtonClick()
    {
        if(!isPaused)
        {
            BT_Clik_Sound(false);
            Advertising.HideBannerAd();
            PauseText.SetActive(true);
            PauseHomeBtn.SetActive(true);
            Time.timeScale = 0;
            AudioListener.volume = 0;
            isPaused = true;
        }
        else
        {
            BT_Clik_Sound(true);
            PauseText.SetActive(false);
            PauseHomeBtn.SetActive(false);
            AudioListener.volume= PlayerPrefs.GetInt("Sound");
            Time.timeScale = 1;
            isPaused = false;
            if (PlayerPrefs.GetInt("HideAd") != 1)
                Advertising.ShowBannerAd(BannerAdPosition.Bottom);
        }
    }

    void InitiateNewSpawn()
    {
        Invoke("SpawnThrowableItems",0.42f);
    }

    public void SpawnThrowableItems()
    {
        if (isGameStarted)
            TotalAmmo = TotalAmmo - 1;
        else
            isGameStarted = true;

        if(TotalAmmo>=0)
            AmmoText.text = TotalAmmo.ToString();

        if (TotalAmmo>0)
        {
            GameObject Go = Instantiate(ThrowableObject, ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].position, ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].rotation);
            Player = Go;
        }
        else if(!CHK_GAME_WIN && Player==null)
        {
            Defeat();
        }
    }

    public void CallBonusRound()
    {
        StartCoroutine(InititateBonusRound());
    }

     IEnumerator InititateBonusRound()
    {
        yield return new WaitForSeconds(2.6f);
        isBonusRound = true;
        AttackBtn.SetActive(true);
        for(int i= (SpawnCastleGO.transform.childCount-3); i< SpawnCastleGO.transform.childCount;i++)
        {
            Destroy(SpawnCastleGO.transform.GetChild(i).gameObject);
        }
        if(levelIndex==9 || levelIndex==10)
            Invoke("Call_Spawn_Enemy", 2f);
        else
            InvokeRepeating("Call_Spawn_Enemy", 0,2f);
    }

    public void Call_Spawn_Enemy()
    {
        if (isBonusRound)
        {
            float WaitTime = Random.Range(0, 1f);
            StartCoroutine(SpawnEnemy(WaitTime));
        }
    }

    IEnumerator SpawnEnemy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (EnemyManager.insance.EnemySpawn.Count > 0)
        {
            //Random Enemies
            int EnemyIndex = Random.Range(0, EnemyManager.insance.EnemySpawn.Count);
            Instantiate(EnemyManager.insance.EnemySpawn[EnemyIndex], EnempySpawnPos.position, Quaternion.identity);
        }
        else
            print("No Enemies");
    }

    void CallFinalVictoryScreen()
    {
        Victory(1);
    }

    public void Victory(int LevelPhase)
    { 
        CHK_GAME_WIN = true;
        if (LevelPhase == 0)
        {
            if (Sound_Manager.instance)
                Sound_Manager.instance.fireWork.Play();

            aimingRangeBtn.tween.Play();
            fillBar.tween.Play();

            TrajectoryDots.SetActive(false);
            winParticle.SetActive(true);
            VictoryText.SetActive(true);

            buttonText.text = "NEXT";
            
            levelIndex = levelIndex + 1;
            if (levelIndex >= environments.Length)
                levelIndex = 0;

            PlayerPrefs.SetInt("Lvl_Index", levelIndex);//Current Lvl Index Change
            PlayerPrefs.SetInt("Level" + (levelIndex + 1), 1);//Next Level Unlock
            Check_Power_Upgraded();
            Invoke("DisableVictoryText",2f);
        }
        else if (LevelPhase == 1)
        {
            Advertising.HideBannerAd();
            stopSounds();
            if (Sound_Manager.instance)
                Sound_Manager.instance.victory.Play();

            Result.SetActive(true);
            AttackBtn.SetActive(false);
            ScoreText.text = PlayerScore.ToString();
            ResultStatusIcon.sprite = ResultImages[1];
            InGameHomeMenu.SetActive(false);
        }
    }

    void DisableVictoryText()
    {
        if (Sound_Manager.instance)
            Sound_Manager.instance.fireWork.Stop();

        if (PlayerPrefs.GetInt("HideAd") != 1)
            Chartboost.showInterstitial(CBLocation.Default);
        VictoryText.SetActive(false);

        if(TrajectoryDots.activeInHierarchy)
        TrajectoryDots.SetActive(false);
    }

    public void decreaseHealthOfCastle(int weaponType, trajectoryScript ammo)
    { 
        currentCastleHP -= MachineryManager.instance.machineryDamage;
        if (PlayerPrefs.GetInt("Machinery" + weaponType + "_Upgraded") == 1 && PlayerPrefs.GetInt("Machinery" + weaponType + "_Exhausted") != 1)
        {
            currentCastleHP -= MachineryManager.instance.machineryDamage;
        }

        if (currentCastleHP <= 0 && !GameOver)
        {
            ammo.Blast();
            PlayerScore = PlayerScore + MachineryManager.instance.machineryHP;
            GameOver = true;
            for (int i = 0; i < Flame.Length - 1; i++)
            {
                Flame[i].transform.localScale = new Vector3(6.2f, 6.2f, 6.2f);
            }
            Flame[6].SetActive(true);
            Victory(0);

            EnemyManager.insance.StopEnemyShooting();

            CallBonusRound();

            if (Player != null)
            {
                if (weaponType == 5)//M777A2
                {
                    //print("M777A2");
                    /*if(GameManager.instance.Player)
                        Destroy(GameManager.instance.Player.transform.parent.gameObject);*/
                }
                else
                {
                    Destroy(GameManager.instance.Player);
                }
            }
            ammo.trajectoryDots.SetActive(false);//new
            return;
        }
        else if (currentCastleHP > 0 && !GameOver && !CHK_GAME_WIN)
        {
            float temp = ((float)MachineryManager.instance.machineryDamage / levelCastleHP);
            CastleHealthFillbar.fillAmount -= temp;
            if (PlayerPrefs.GetInt("Machinery" + weaponType + "_Upgraded") == 1 && PlayerPrefs.GetInt("Machinery" + weaponType + "_Exhausted") != 1)
            {
                CastleHealthFillbar.fillAmount -= temp;
            }
            PlayerScore = PlayerScore + MachineryManager.instance.machineryHP;

            ammo.Blast();
        }

        if (CastleHealthFillbar.fillAmount < 0.5f && CastleHealthFillbar.fillAmount > 0.35f)
        {
            for (int i = 0; i < Flame.Length - 1; i++)
            {
                Flame[i].SetActive(true);
            }
            if(Sound_Manager.instance)
            {
                Sound_Manager.instance.castelFire.Play();
            }
        }
        else if (CastleHealthFillbar.fillAmount < 0.35f && CastleHealthFillbar.fillAmount > 0.25f)
        {
            for (int i = 0; i < Flame.Length - 1; i++)
            {
                Flame[i].transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
            }
            if (Sound_Manager.instance)
            {
                Sound_Manager.instance.castelFire.volume=0.8f;
            }
        }
        else if (CastleHealthFillbar.fillAmount < 0.25f)
        {
            for (int i = 0; i < Flame.Length - 1; i++)
            {
                Flame[i].transform.localScale = new Vector3(6.2f, 6.2f, 6.2f);
            }
            if (Sound_Manager.instance)
            {
                Sound_Manager.instance.castelFire.volume =1f;
            }
        }
    }

    public void CheckAmmo(GameObject prevAmmo, trajectoryScript current, float delay)
    {
        IsDustParticles = false;

        if (TotalAmmo == 0)
        {
            EnemyManager.insance.StopEnemyShooting();

            if (currentCastleHP <= 0 && CHK_GAME_WIN)
            {
                print("Victory");
                Victory(0);
            }
            else if (!CHK_GAME_WIN)
            {
                GameOver = true;
                Debug.Log("GAMMOVOER");
                Defeat();
            }
        }
        else
        {
            current.enabled = false;
            StartCoroutine(CheckForPlayerNullInit(delay));
            Destroy(prevAmmo);
        }
    }

    public void CheckResult()
    {
        print("Ammo is 0");
        EnemyManager.insance.StopEnemyShooting();

        if (currentCastleHP <= 0 && CHK_GAME_WIN)
        {
            print("Victory");
            Victory(0);
        }
        else if (!CHK_GAME_WIN)
        {
            GameOver = true;
            Debug.Log("GAMMOVOER");
            Defeat();
        }
    }

    public void Defeat()
    {
        if (!CHK_GAME_WIN)
        {
            Advertising.HideBannerAd();
            stopSounds();

            if (Sound_Manager.instance)
                Sound_Manager.instance.defeat.Play();

            PlayerScore = 0;
            isDefeat = true;
            if (Player)
                Destroy(Player.gameObject);

            buttonText.text = "RETRY";
            Result.SetActive(true);
            ScoreText.text = PlayerScore.ToString();
            ResultStatusIcon.sprite = ResultImages[0];
            InGameHomeMenu.SetActive(false);

            Check_Power_Upgraded();
        }
    }

    void stopSounds()
    {
        if (Sound_Manager.instance)
        {
            Sound_Manager.instance.BgMusic.Stop();
            Sound_Manager.instance.castelFire.Stop();
        }
    }

    void Check_Power_Upgraded()
    {
        if (PlayerPrefs.GetInt("Machinery" + MachineryManager.instance.machineryIndex + "_Upgraded") == 1)
        {
            PlayerPrefs.SetInt("Machinery" + MachineryManager.instance.machineryIndex + "_Exhausted", 1);
        }

        if(CHK_GAME_WIN)
        {
            if (PlayerPrefs.GetFloat("Bonus_Time") != 15)
            {
                PlayerPrefs.SetFloat("Bonus_Time", 15);
                PlayerPrefs.SetInt("Bonus_Time_No", 0);
            }

            if(PlayerPrefs.GetInt("Bonus_Multiplier")!= 1)
            {
                PlayerPrefs.SetInt("Bonus_Multiplier", 1);
                PlayerPrefs.SetInt("Bonus_Multiplier_No", 0);
            }
        }
    }
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.GetComponent<Destroy1>().DeactivateInSecs();
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
    #endregion

    public void BT_Sound_Click()
    {
        if (soundIndex == 0)
            soundIndex = 1;
        else
            soundIndex = 0;

        PlayerPrefs.SetInt("Sound", soundIndex);
        AudioListener.volume = soundIndex;
        soundBtn.sprite = soundSprites[soundIndex];
        BT_Clik_Sound(true);
    }

    public void BT_Clik_Sound(bool positive)
    {
        if (Sound_Manager.instance)
        {
            if(positive)
            {
                Sound_Manager.instance.btnPositiveClick.Play();
            }
            else
            {
                Sound_Manager.instance.btnNegativeClick.Play();
            }
        }
    }

    public void Bt_add_Click()
    {
        //Store.instance.PurchaseSampleProduct(strAdHide);
        Unlock_Product();
    }

    public void Unlock_Product()
    {
        PlayerPrefs.SetInt("HideAd", 1);
        Advertising.HideBannerAd();
        hideBtn.interactable = false;
    }

    private void OnDestroy()
    {
        if(CHK_GAME_WIN)
            PlayerPrefs.SetInt("InGameCoins", PlayerPrefs.GetInt("InGameCoins") + PlayerScore);
    }
}
