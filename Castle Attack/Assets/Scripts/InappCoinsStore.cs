using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InappCoinsStore : MonoBehaviour
{
	public float duration = 0.5f;
	int score = 0;
	public int TotalCoinsInt;
	public Text TotalCoinsText;
	public GameObject LoadingBG;
	public GameObject purchased;
	public static InappCoinsStore isn { get; set; }
	// Start is called before the first frame update
	void Awake()
	{
		isn = this;
	}
		void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
	public void BuyCoins()//(GameObject _go)
	{

		//switch (_go.name)
		//{
		//	case "Buy1000":
		//		PlayerPrefs.SetFloat("MPGeneralPlayerMoney", PlayerPrefs.GetFloat("MPGeneralPlayerMoney") + 1000f);
		//		int temp = (int)PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
		//		//TotalCoinsInt = (int)PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
		//		StopCoroutine("CountTo");
		//		StartCoroutine("CountTo", temp);
		//		break;
		//case "Buy5000":
		//	PlayerPrefs.SetFloat("MPGeneralPlayerMoney", PlayerPrefs.GetFloat("MPGeneralPlayerMoney") + 5000f);
		//	int temp1 = (int)PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
		//	//TotalCoinsInt = (int)PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
		//	StopCoroutine("CountTo");
		//	StartCoroutine("CountTo", temp1);
		//	break;
		//}
		PlayerPrefs.SetFloat("MPGeneralPlayerMoney", PlayerPrefs.GetFloat("MPGeneralPlayerMoney") + 1000f);
        int temp = (int)PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
        TotalCoinsInt = (int)PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
		TotalCoinsText.text = TotalCoinsInt.ToString();
		purchased.SetActive(true);
		//StopCoroutine("CountTo");
		//StartCoroutine("CountTo", temp);
	}

	IEnumerator CountTo(int target)
	{
		int start = TotalCoinsInt;
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
		{
			float progress = timer / duration;
			TotalCoinsInt = (int)Mathf.Lerp(start, target, progress);
			TotalCoinsText.text = TotalCoinsInt.ToString();
			yield return null;
		}
		TotalCoinsInt = target;
		TotalCoinsText.text = TotalCoinsInt.ToString();

	}
}
