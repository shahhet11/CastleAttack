using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class Store : MonoBehaviour
{

    public string[] localProductID;
    public static Store instance;

    void OnEnable()
    {
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        /*if(!RuntimeManager.IsInitialized())
        {
            RuntimeManager.Init();
        }*/
    }

    void Start()
    {
        bool isInitialized = InAppPurchasing.IsInitialized();
    }


    public void FetchProducts()
    {
        IAPProduct[] products = InAppPurchasing.GetAllIAPProducts();
        foreach (IAPProduct prod in products)
        {
            Debug.Log("Product name: " + prod.Name  + " - "+prod.Id);
        }
    }

    public void PurchaseSampleProduct(string productID)
    {
        Debug.Log("PurchaseSampleProduct "+ productID);
        InAppPurchasing.PurchaseWithId(productID);
    }

    void PurchaseCompletedHandler(IAPProduct product)
    {
        Debug.Log("Purchase Complete    "+product.Name + " - "+product.Id);
        // Compare product name to the generated name constants to determine which product was bought
        /* switch (product.Name)
         {
             case EM_IAPConstants.Product_FireBall:
                 Debug.Log(product.Name + " - " + product.Id);

                 Debug.Log("Product_FireBall was purchased. The user should be granted it now.");
                 UIScript.instance.FreeTheMachine(PlayerPrefs.GetInt("ProductIndex"));
                 break;
             case EM_IAPConstants.Product_Spartan:
                 Debug.Log(product.Name + " - " + product.Id);

                 Debug.Log("Product_Spartan was purchased. The user should be granted it now.");
                 UIScript.instance.FreeTheCharacter(PlayerPrefs.GetInt("ProductIndex"));
                 break;
                 // More products here...
         }*/
        if (UIScript.instance)
            UIScript.instance.Unlock_Product();
        else
            GameManager.instance.Unlock_Product();
    }

    // Failed purchase handler
    void PurchaseFailedHandler(IAPProduct product,string reason)
    {
        Debug.Log("The purchase of product " + product.Name + " has failed.");
    }

    void OnDisable()
    {
        InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
    }
}
