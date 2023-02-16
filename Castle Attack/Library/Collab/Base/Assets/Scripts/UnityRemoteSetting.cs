using UnityEngine;

public class UnityRemoteSetting : MonoBehaviour
{
    public static UnityRemoteSetting isn { get; set; }

   // public UnityEngine.UI.Text text;

    public string coin1000IAP;

  

    void Awake()
    {
        isn = this;
        RemoteSettings.Updated += new RemoteSettings.UpdatedEventHandler(HandleRemoteUpdate);
        //HandleRemoteUpdate();
        Debug.Log("URS");
    }

    private void HandleRemoteUpdate()
    {
        
        GetComponent<UnityIAP>().enabled = false;
#if UNITY_ANDROID
        coin1000IAP = RemoteSettings.GetString("coin1000IAP_Android");
        
#elif UNITY_IOS
        coin1000IAP = RemoteSettings.GetString("coin1000IAP_IOS");
       
#endif




          if (PlayerPrefs.GetString("coin1000IAP", "null") != coin1000IAP)
            PlayerPrefs.SetString("coin1000IAP", coin1000IAP);

 
        


        Debug.Log(PlayerPrefs.GetString("coin1000IAP"));

        GetComponent<UnityIAP>().enabled = true;
    }
}