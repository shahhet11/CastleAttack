using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class sounds
{
    public AudioClip fire, hit;
}

public class Sound_Manager: MonoBehaviour
{
    public AudioSource playerFire, playerHit,enemyFire,enemyHit, BgMusic,castelFire,fireWork,defeat,victory,btnPositiveClick,btnNegativeClick;
    public static Sound_Manager instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    
    public void CheckMachinery()
    {
        playerFire.clip = MachineryManager.instance.lstMachineryData[PlayerPrefs.GetInt("Selected_Mac")].fireClip;
        playerHit.clip = MachineryManager.instance.lstMachineryData[PlayerPrefs.GetInt("Selected_Mac")].hitClip;

        enemyFire.clip = GameManager.instance.levelsData[GameManager.instance.levelIndex].fireClip;
        enemyHit.clip = GameManager.instance.levelsData[GameManager.instance.levelIndex].hitClip;
    }
}
