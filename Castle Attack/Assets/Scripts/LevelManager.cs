using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {

        public string LevelText;
        public int Unlock;
        public bool isInteractible;

        public Button.ButtonClickedEvent OnClick;
    }


    public GameObject LEVELButton;
    public Transform Spacer;
    public List<Level> LevelList;

    public static int selectedLevelNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        PrerequisiteData();
        FillList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FillList()
    {
        foreach (var level in LevelList)
        {

            GameObject newbutton = Instantiate(LEVELButton, Spacer) as GameObject;
            newbutton.transform.localScale = new Vector3(1, 1, 1);
            level_button_new button = newbutton.GetComponent<level_button_new>();

            button.LevelText.text = level.LevelText;

            if (PlayerPrefs.GetInt("Level" + button.LevelText.text) == 1)
            {
                level.Unlock = 1;
                level.isInteractible = true;
            }

            button.unlocked = level.Unlock;
            button.GetComponent<Button>().interactable = level.isInteractible;
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(button.LevelText.text));
        }
        SAVE();
    }

    void SAVE()
    {
        {
            GameObject[] allbuttons = GameObject.FindGameObjectsWithTag("LevelButton");
            foreach (GameObject buttons in allbuttons)
            {
                level_button_new button = buttons.GetComponent<level_button_new>();
                PlayerPrefs.SetInt("Level" + button.LevelText.text, button.unlocked);
            }
        }
    }

    void LoadLevel(string value)
    {
        selectedLevelNumber = int.Parse(value);
        //Debug.Log("sel level : " + selectedLevelNumber);
        UIScript.instance.LevelUI.SetActive(false);
        UIScript.instance.WeaponsUI.SetActive(true);
        CharacterManager.instance.ShowCharacterAsPerLevel(selectedLevelNumber);
        MachineryManager.instance.ShowMachineAsPerLevel(selectedLevelNumber);

    }

    void PrerequisiteData()
    {
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Level1", 1);
            PlayerPrefs.SetInt("InGameCoins", 100);
            PlayerPrefs.SetInt("Character0", 1);
            PlayerPrefs.SetInt("Machinery0", 1);
            PlayerPrefs.SetInt("Sound", 1);


            PlayerPrefs.SetInt("FirstTime", 1111);
        }
    }
}
