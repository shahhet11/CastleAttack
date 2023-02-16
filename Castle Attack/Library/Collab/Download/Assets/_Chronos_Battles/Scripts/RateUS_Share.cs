using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RateUS_Share : MonoBehaviour
{
	string GAME_NAME = "share message";
	string App_URL = "http://www.google.com";
	string MOREGAME_URL = "http://www.google.com";

	public void User_Share_Spacific(string url)
    {
		Application.OpenURL(url);
    }
	public void User_Share_TextURL ()
	{
		shareApp ();
	}

	public void User_RateUS_Click ()
	{
		Application.OpenURL ("https://play.google.com/store/apps/details?id=" + Application.identifier);

		PlayerPrefs.SetInt ("RATE_CLICK", 1);
	}

	public void User_MoreGame_Click ()
	{
		Application.OpenURL ("https://play.google.com/store/apps/developer?id=" + Application.companyName);
	}

	public static void shareApp ()
	{
		#if UNITY_ANDROID
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Download " + Application.productName + " from https://play.google.com/store/apps/details?id=" + Application.identifier + "&hl=en");
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);
		#elif UNITY_IOS    	
		GeneralSharingiOSBridge.ShareSimpleText ("Download "+Application.productName+" from https://play.google.com/store/apps/details?id="+Application.identifier+"&hl=en");
		#endif
	}

	public void QuitApp()
	{
		Application.Quit();
	}
	
}
