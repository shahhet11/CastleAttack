using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    //    public Transform originalObject;
    //    public Transform reflectedObject;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public bool fingertouch = false;
    public bool IsDustParticles = false;
    public GameObject[] Flame;
    public GameObject Player;
    public Vector2 CurrentAmmoShotForce;

    //public GameObject CharacterPlayer;
    public GameObject BlastPrefab;
    public GameObject BlastPrefabCannon;
    public GameObject BloodParticle;
    public GameObject ThrowableObject;
    public GameObject CurrentThrowableObject;
    public Transform[] ThrowableSpawnPosition;
    public bool isPaused;
    public GameObject PauseText;
    public GameObject PauseHomeBtn;
    public Animator animator;
    public GameObject[] dots;
    public bool SpawnNew;
    public static GameManager instance;
    public Animation AmmoCounter;
    public int TotalAmmo = 10;
    public Text AmmoText;
    public GameObject Result;
    public Image ResultStatusIcon;
    public Sprite[] ResultImages;
    public int CastleHealthCount = 100;
    public Image CastleHealthFillbar;
    public float CastleFill;
    public int CatapultHealthCount = 100;
    public Image CatapultHealthFillbar;
    public float CatapultFill;
    public bool GameOver;
    public int PlayerScore = 0;
    public int InGameCoins = 0;
    public Text ScoreText;
    public GameObject TrajectoryDots;

    public GameObject InGameHomeMenu;

    public int currentCastleHP = 0;
    public int currentMachineryHP = 0;
    public int ThisMachineryHP = 0;

    [Header("[ MACHINERY DATA VAR ]")]
    public List<MachinerysData> machinerysDatas;
    public GameObject MachinerySpawnPos;
    public int MachineryIndex;
    // Level Data Varaiables...
    [Header("[ LEVEL DATA VAR ]")]
    public Transform trLevelCastleParent;
    
    public Text textLevelNo, textCastleData;
    public GameObject VictoryText;
    public List<LevelsData> levelsData;
    public int selLevelNo;
    public int levelCastleHP;
    public int thislevelCastleHP;
    public int levelCastleDamage;
    public int levelCastleReward;
    public bool levelIsLock;
    public GameObject goLevelCastle;
    public Transform CastleSpawnPosition;
    public int levelCastleMachinery;
    public GameObject goLevelCastleAmmo;
    public int levelIndex = 0;
   private GameObject SpawnCastleGO;
    public GameObject goCharacterSprite;
    public bool isDefeat;
    [Header("[ Player DATA VAR ]")]
    public List<CharactersData> characterData;
    public Transform trPlayerCharacterParent;
    public GameObject goLevelCharacter;
    public int characterIndex = 0;
    public GameObject CharacterSpawnPos;
    GameObject storeChar;
    public Button AttackBtnClick;
    //// Machinery Data Variables...
    //public Transform trLevelMachineryParent;
    //public List<MachinerysData> machineryData;
    //public int selMachineryNo;
    //public int machineryDamage;
    //public int machineryHP;
    //public string machineryName;
    //int machineryIndex = 0;

    [Header("[GAME OVER]")]
    public Image soundBtn;
    public Sprite[] soundSprites;
    int soundIndex;
    public Text coordsTExt;



    [Header("[ BONUS ROUND ]")]
    public GameObject AttackBtn;
    public float Timer = 15;
    public bool isBonusRound;
    public Transform EnempySpawnPos;
    public bool CHK_GAME_WIN = false;

    #endregion

    #region -- Unity Default Methods--
    // Start is called before the first frame update
    void Start()
    {
        characterIndex = CharacterManager.instance.characterIndex;
        //// Set Machinery Data From Data Container
        //machineryIndex = (LevelManager.selectedLevelNumber - 1);
        //selMachineryNo = machineryData[machineryIndex].MachineryNo;
        //machineryDamage = machineryData[machineryIndex].Damage;
        //machineryHP = machineryData[machineryIndex].HP;
        //currentMachineryHP = machineryHP;
        //machineryName = machineryData[machineryIndex].MachineryName;
        //ThrowableObject = machineryData[machineryIndex].GoAmmo;
        TotalAmmo = 10;
        SetMachinery();
        InGameCoins = PlayerPrefs.GetInt("InGameCoins");
        SpawnThrowableItems();
        //goCharacterSprite.GetComponent<SpriteRenderer>().sprite = CharacterManager.instance.characterSprite;
        
        // Set Level Data From Data Container
        levelIndex = (LevelManager.selectedLevelNumber-1);
        MachineryIndex = ( MachineryManager.instance.selMachineryNo - 1);
        selLevelNo = levelsData[levelIndex].LevelNo;
        levelCastleDamage = levelsData[levelIndex].CastleDamage;
        thislevelCastleHP = levelsData[levelIndex].CastleHP;
        levelCastleHP = levelsData[levelIndex].CastleHP;
        levelCastleReward = levelsData[levelIndex].CastleReward;
        levelIsLock = levelsData[levelIndex].IsLock;
        levelCastleMachinery = levelsData[levelIndex].CastleMachinery;
        goLevelCastle = levelsData[levelIndex].GoCastle;
        goLevelCastleAmmo = levelsData[levelIndex].GoCastleAmmo;
        goLevelCharacter = characterData[characterIndex].CharacterPrefab;
        storeChar = Instantiate(goLevelCharacter, CharacterSpawnPos.transform.position, Quaternion.identity);
        storeChar.transform.SetParent(trPlayerCharacterParent); 
        EnemyManager.insance._WeaponType = levelsData[levelIndex]._WeaponType;
        SetLevel();
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

        CastleFill = 1f;
        CatapultFill = 1f;
        //CountDown(5f);
        //animator.SetInteger("Catapult",1);
        

        InvokeRepeating("FingerTouchParticles", 1f, 0.1f);
        //InvokeRepeating("DustParticlesTrail", .01f, .05f);

        //=== Sound
        soundIndex = PlayerPrefs.GetInt("Sound");
        AudioListener.volume = soundIndex;
        soundBtn.sprite = soundSprites[soundIndex];
    }

   
    public void CheckForPlayerNull()
    {
        StartCoroutine(CheckForPlayerNullInit());
    }
    IEnumerator CheckForPlayerNullInit()
    {
        yield return new WaitForSeconds(2);
        //Debug.Log("Checking For Null");
        if(Player == null)
        {
            //Debug.Log("Checking For Nul1l1");
            if (SpawnNew == false)
            {
               // Debug.Log("Checking For Null2");
                SpawnNew = true;
            }
        }
    }
    void NextLevelData(int ChangelevelIndex)
    {
        //Debug.Log("NextLevelData"+ levelIndex);

        for (int i = 4; i < 7; i++)
        {

        Destroy(SpawnCastleGO.transform.GetChild(i).gameObject);
        }
        
        levelIndex = ChangelevelIndex;
        int Levelno = levelIndex + 1;
       /// Debug.Log(PlayerScore + "PlayerSCore");
        PlayerPrefs.SetInt("InGameCoins", PlayerPrefs.GetInt("InGameCoins") + PlayerScore);
        PlayerPrefs.SetInt("Level"+ Levelno.ToString() , 1);
        selLevelNo = levelsData[ChangelevelIndex].LevelNo;
        thislevelCastleHP = levelsData[ChangelevelIndex].CastleHP;
        levelCastleDamage = levelsData[ChangelevelIndex].CastleDamage;
        levelCastleHP = levelsData[ChangelevelIndex].CastleHP;
        levelCastleReward = levelsData[ChangelevelIndex].CastleReward;
        levelIsLock = levelsData[ChangelevelIndex].IsLock;
        levelCastleMachinery = levelsData[ChangelevelIndex].CastleMachinery;
        goLevelCastle = levelsData[ChangelevelIndex].GoCastle;
        goLevelCastleAmmo = levelsData[ChangelevelIndex].GoCastleAmmo;
        //SetLevel();
    }

    public void OnNextButtonClick()
    {
       // SpawnNew = false;
        for (int i = 0; i < 3; i++)
        {
            if (EnemyManager.insance.CastleMachineryAnimator[i] != null)
            Destroy(EnemyManager.insance.CastleMachineryAnimator[i].transform.parent.gameObject);
        }
        
        SetLevel();
        TotalAmmo = 11;
        ResetPrerequisites();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player)
            CurrentAmmoShotForce = Player.GetComponent<trajectoryScript>().shotForce;
        /*if (TotalAmmo == 0)
        {
            if (Player != null && Player.GetComponent<trajectoryScript>().isThrown)
                return;
            //Result Status
            EnemyManager.insance.StopEnemyShooting();
            GameOver = true;
            if (currentCastleHP > 0)
            {
                Debug.Log("AMMOVOER");
                Defeat();
            }
            else
            {
                Victory(0);
            }
            
          
            return;
        }*/

        if (SpawnNew && !GameOver)
        {
            //print(SpawnNew + "    " + GameOver);
            //TrajectoryDots.SetActive(true);
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
                CancelInvoke("BonusRoundStarted");
                
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
            EnemyManager.insance.EnemySpawn[i] = levelsData[levelIndex].GoEnemies[i];
        }
        TotalAmmo = machinerysDatas[levelIndex].Ammo;
        
        AmmoText.text = TotalAmmo.ToString();
        currentMachineryHP = machinerysDatas[MachineryIndex].HP;
        ThisMachineryHP = currentMachineryHP;
        textLevelNo.text = "Level " + selLevelNo +" \nMachinery \n\t"+ currentMachineryHP +"-"+MachineryManager.instance.machineryDamage +"\n\t"+MachineryManager.instance.machineryName +"\n Player \n\t"+CharacterManager.instance.characterName;

        if (SpawnCastleGO == null)
        {
        SpawnCastleGO = Instantiate(goLevelCastle, CastleSpawnPosition.position, CastleSpawnPosition.rotation);
        SpawnCastleGO.transform.parent = trLevelCastleParent;
        }
        //Debug.Log(levelIndex + "levelIndex");
       
        for (int i = 0; i < 3; i++)
        {
        //SpawnCastleGO.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = levelsData[levelIndex].SpriteCastleMachinery;
          GameObject Go =  Instantiate(levelsData[levelIndex].CastleMachineryPrefab, SpawnCastleGO.transform.GetChild(i).transform.position, Quaternion.identity);
            EnemyManager.insance.CastleMachineryAnimator[i] = Go.transform.Find("_Animator").GetComponent<Animator>();
            Go.transform.SetParent(SpawnCastleGO.transform);
        }
         
        currentCastleHP = levelCastleHP;
        
        CastleHealthFillbar.fillAmount = 1;
        CatapultHealthFillbar.fillAmount = 1;
        textCastleData.text = "HP: " + currentCastleHP + " - " + levelCastleDamage;
    }
    void ResetPrerequisites()
    {
        PlayerScore = 0;
        for (int i = 0; i < Flame.Length - 1; i++)
        {
            Flame[i].transform.localScale = new Vector3(2.704775f, 2.704775f, 2.704775f);
            Flame[i].SetActive(false);
        }
        Flame[6].SetActive(false);
        //SpawnThrowableItems();
        InGameHomeMenu.SetActive(true);
        GameOver = false;
        if (isDefeat)
        {
            SpawnNew = true;
            isDefeat = false;
        }
        EnemyManager.insance._WeaponType = levelsData[levelIndex]._WeaponType;
        EnemyManager.insance.StartEnemyShooting();

    }
    void SetMachinery()
    {
        currentMachineryHP = MachineryManager.instance.machineryHP;
        ThrowableObject = MachineryManager.instance.goMachineryAmmo;
        //goMachinerySprite.GetComponent<SpriteRenderer>().sprite = MachineryManager.instance.spriteMachinery;
        Instantiate(MachineryManager.instance.goMachineryPrefab, MachinerySpawnPos.transform.position, Quaternion.identity);
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
            //             go = Instantiate(FingerTouchPE, hit.point, Quaternion.identity);

        }
    }
    void DustParticlesTrail()
    {
        if (IsDustParticles != false)
        {
            //Debug.Log("Received");
            SpawnFromPool("DustParticle", Player.transform.position, Quaternion.identity);

        }

    }

    public void OnHomeButtonClick()
    {
        if(Time.timeScale != 1)
            Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }
    public void OnPauseButtonClick()
    {
        if(!isPaused)
        {
            PauseText.SetActive(true);
            PauseHomeBtn.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            PauseText.SetActive(false);
            PauseHomeBtn.SetActive(false);

            Time.timeScale = 1;
            isPaused = false;
        }

    }
    void InitiateNewSpawn()
    {
        Invoke("SpawnThrowableItems",0.42f);
    }
    public void SpawnThrowableItems()
    {
       // print("SpawnThrowable");
        //Debug.Log((int)MachineryManager.instance._WeaponType + "WEAPONTYPE VALUE");
        TotalAmmo = TotalAmmo - 1;
        AmmoText.text = TotalAmmo.ToString();

        //print("Total Ammo " + TotalAmmo);
        if (TotalAmmo>0)
        {
            GameObject Go = Instantiate(ThrowableObject, ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].position, ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].rotation);
            Player = Go;
        }
        //else
        //{
        //    EnemyManager.insance.StopEnemyShooting();

        //    if (currentCastleHP <= 0 && !CHK_GAME_WIN)
        //    {
        //        print("Victory");
        //        Victory(0);
        //    }
        //    else if (!CHK_GAME_WIN)
        //    {
        //        GameOver = true;
        //         Debug.Log("GAMMOVOER");
        //        Defeat();
        //    }
        //    return;
        //}
    }

    public void CallBonusRound()
    {
        StartCoroutine(InititateBonusRound());
    }

     IEnumerator InititateBonusRound()
    {
       // Debug.Log("BONUSROUND0");
        yield return new WaitForSeconds(2.6f);
        //Debug.Log("BONUSROUND1");
        isBonusRound = true;
        AttackBtn.SetActive(true);
        InvokeRepeating("BonusRoundStarted", 0,2f);

    }
    //void StartBonusRound()
    //{

    //}

    void BonusRoundStarted()
    {
        float WaitTime = Random.Range(0,2f);
        StartCoroutine(SpawnEnemy(WaitTime));
    }
    IEnumerator SpawnEnemy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // Random Enemies
        int EnemyIndex = Random.Range(0, 3);
        Instantiate(EnemyManager.insance.EnemySpawn[EnemyIndex] , EnempySpawnPos.position , Quaternion.identity);
        
    }
    void CallFinalVictoryScreen()
    {
        Victory(1);
    }
    public void Victory(int LevelPhase)
    {
        // LevelPhase - 0 => Victory Text
        // LevelPhase - 1 => Bonus Round Win

        CHK_GAME_WIN = true;
        if (LevelPhase == 0)
        {
            VictoryText.SetActive(true);
            Invoke("DisableVictoryText",2.5f);
        }
        else if (LevelPhase == 1)
        {
         Result.SetActive(true);
            AttackBtn.SetActive(false);
            ScoreText.text = PlayerScore.ToString();
        ResultStatusIcon.sprite = ResultImages[1];
        InGameHomeMenu.SetActive(false);
            levelIndex = levelIndex + 1;
            NextLevelData(levelIndex);
        }
        //Cal Score
    }
    void DisableVictoryText()
    {
        VictoryText.SetActive(false);

        if(TrajectoryDots.activeInHierarchy)
        TrajectoryDots.SetActive(false);
    }
    public void Defeat()
    {
        if (!CHK_GAME_WIN)
        {
            PlayerScore = 0;
            //StopCoroutine(EnemyManager.insance.EnemyThrowRock());
            isDefeat = true;
            if (Player)
                Destroy(Player.gameObject);

            Result.SetActive(true);
            ScoreText.text = PlayerScore.ToString();
            ResultStatusIcon.sprite = ResultImages[0];
            InGameHomeMenu.SetActive(false);
        }
      
        //Cal Score
    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            //Debug.LogWarning("Pool with tag" + tag + "dosent exist");
            return null;
        }
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

















    //***************************************EXTRAS**********************************//

    //void CountDown(float time)
    //{

    //    StartCoroutine("CountDownAnimation", time);


    //}

    //IEnumerator CountDownAnimation(float time)
    //{
    //    float animationTime = time;
    //    while (animationTime > 0)
    //    {
    //        animationTime -= Time.deltaTime;
    //        CastleHealthFillbar.fillAmount = animationTime / time;
    //        yield return null;
    //    }
    //}

    //void CallInUPdate()
    //{
    //    // Opti
    //    if (CastleHealthFillbar.fillAmount <= CastleHealthCount / 100)
    //    {
    //        CastleFill -= Time.deltaTime * 0.1f;

    //        CastleHealthFillbar.fillAmount = CastleFill;
    //    }
    //}
}
