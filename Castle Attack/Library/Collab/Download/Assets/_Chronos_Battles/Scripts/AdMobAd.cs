using UnityEngine;
using EasyMobile;

public class AdMobAd : MonoBehaviour
{
    public static AdMobAd instance;
    //public BannerView bannerView;
    //public string androidAdId;

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
    void Start()
    {


       Advertising.Initialize();
/*#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-xxxxxxxxxx";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-xxxxxxxxxx";
#else
        string adUnityId = "unexpected_platform";
#endif*/


        //Debug.Log(bannerView);
       /* bannerView = new BannerView(androidAdId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
        bannerView.Show();
        //HideBans();

        Advertising.bann*/
        //Advertising.ShowBannerAd(BannerAdPosition.Bottom);

    }

    public void HideBans()
    {
        //Debug.Log("Hide");
        //bannerView.Hide();
        Debug.Log("Hide");
    }

    public void ShowBans()
    {
        //bannerView.Show();
        Debug.Log("Show");
    }
}