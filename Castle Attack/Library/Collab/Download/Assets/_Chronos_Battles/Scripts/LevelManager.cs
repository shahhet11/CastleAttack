using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public static int selectedLevelNumber = 0;

    public Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        PrerequisiteData();
        FillList();
    }

    void FillList()
    {
        for(int i=0;i<buttons.Length;i++)
        {
            if (PlayerPrefs.GetInt("Level" + (i + 1)) == 1)
            {
                buttons[i].interactable = true;
                buttons[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                buttons[i].interactable = false;
                buttons[i].GetComponent<Image>().color = new Color(.7f, .7f, .7f);
            }
        }
       
    }

    public void LoadLevel(int value)
    {
        PlayerPrefs.SetInt("Lvl_Index", (value - 1));
        UIScript.instance.LevelUI.SetActive(false);
        UIScript.instance.WeaponsUI.SetActive(true);
        CharacterManager.instance.ShowCharacterAsPerLevel(value);
        MachineryManager.instance.ShowMachineAsPerLevel(value);
    }

    void PrerequisiteData()
    {
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.DeleteAll();

            PlayerPrefs.SetInt("Level1", 1);//Free
            //PlayerPrefs.SetInt("Level2", 1);
            //PlayerPrefs.SetInt("Level3", 1);
            //PlayerPrefs.SetInt("Level4", 1);
            //PlayerPrefs.SetInt("Level5", 1);
            //PlayerPrefs.SetInt("Level6", 1);
            //PlayerPrefs.SetInt("Level7", 1);
            //PlayerPrefs.SetInt("Level8", 1);
            //PlayerPrefs.SetInt("Level9", 1);
            //PlayerPrefs.SetInt("Level10", 1);
            //PlayerPrefs.SetInt("Level11", 1);
            //PlayerPrefs.SetInt("Level12", 1);
            //PlayerPrefs.SetInt("Level13", 1);
            //PlayerPrefs.SetInt("Level14", 1);
            //PlayerPrefs.SetInt("Level15", 1);

            PlayerPrefs.SetInt("Character0", 1);//Free
            //PlayerPrefs.SetInt("Character1", 1);
            //PlayerPrefs.SetInt("Character2", 1);
            PlayerPrefs.SetInt("Character3", 1);//Free
            //PlayerPrefs.SetInt("Character4", 1);
            //PlayerPrefs.SetInt("Character5", 1);
            //PlayerPrefs.SetInt("Character6", 1);
            //PlayerPrefs.SetInt("Character7", 1);
            PlayerPrefs.SetInt("Character8", 1);//Free
            //PlayerPrefs.SetInt("Character9", 1);
            //PlayerPrefs.SetInt("Character10", 1);
            //PlayerPrefs.SetInt("Character11", 1);

            PlayerPrefs.SetInt("Machinery0", 1);//Free
            //PlayerPrefs.SetInt("Machinery1", 1);
            //PlayerPrefs.SetInt("Machinery2", 1);
            //PlayerPrefs.SetInt("Machinery3", 1);
            //PlayerPrefs.SetInt("Machinery4", 1);
            PlayerPrefs.SetInt("Machinery5", 1);//Free
            //PlayerPrefs.SetInt("Machinery6", 1);
            //PlayerPrefs.SetInt("Machinery7", 1);
            //PlayerPrefs.SetInt("Machinery8", 1);
            //PlayerPrefs.SetInt("Machinery9", 1);
            PlayerPrefs.SetInt("Machinery10", 1);//Free
            //PlayerPrefs.SetInt("Machinery11", 1);
            //PlayerPrefs.SetInt("Machinery12", 1);
            //PlayerPrefs.SetInt("Machinery13", 1);
            //PlayerPrefs.SetInt("Machinery14", 1);

            PlayerPrefs.SetInt("HideAd",0);
            PlayerPrefs.SetInt("Selected_Char", 0);
            PlayerPrefs.SetInt("Selected_Mac", 0);
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Lvl_Index", 0);

            PlayerPrefs.SetFloat("Bonus_Time", 15);
            PlayerPrefs.SetInt("Bonus_Time_No", 0);

            PlayerPrefs.SetInt("Bonus_Multiplier", 1);
            PlayerPrefs.SetInt("Bonus_Multiplier_No", 0);

            PlayerPrefs.SetInt("InGameCoins", 0);
            PlayerPrefs.SetInt("Coin_Upgrade_No", 0);

            PlayerPrefs.SetInt("FirstTime", 1111);
        }
    }
}
