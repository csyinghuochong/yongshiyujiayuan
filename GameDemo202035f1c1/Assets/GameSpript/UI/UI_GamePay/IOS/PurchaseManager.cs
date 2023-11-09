using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


public class PurchaseManager : MonoBehaviour, IStoreListener
{

    private IStoreController controller;
    private int count = 0;


    void Awake()
    {
        if (this.GetComponent<UI_RmbStore>().IOSInitStatus == true)
        {
            return;
        }

        Debug.Log("准备开始初始化..");
#if UNITY_IPHONE
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct("198YS", ProductType.Consumable);
        builder.AddProduct("298YS", ProductType.Consumable);
        builder.AddProduct("30YS", ProductType.Consumable);
        builder.AddProduct("488YS", ProductType.Consumable);
        builder.AddProduct("50YS", ProductType.Consumable);
        builder.AddProduct("6YS", ProductType.Consumable);
        builder.AddProduct("648YS", ProductType.Consumable);
        builder.AddProduct("98YS", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);

        Debug.Log("Awake初始化结束..");
        this.GetComponent<UI_RmbStore>().IOSInitStatus = true;
#endif
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
#if UNITY_IPHONE
        this.controller = controller;
        Debug.Log("初始化成功..");
        this.GetComponent<UI_RmbStore>().IOSInitStatus = true;
#endif
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //Panel_Log.my.Error("初始化失败");
        Debug.Log("初始化失败");
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("购买成功ios");

        var unifiedReceipt = JsonUtility.FromJson<UnifiedReceipt>(e.purchasedProduct.receipt);

        if (unifiedReceipt != null && !string.IsNullOrEmpty(unifiedReceipt.Payload))
        {

            Debug.Log("t4=" + unifiedReceipt.Payload.ToString());

            Game_PublicClassVar.Get_game_PositionVar.OnPayResult(unifiedReceipt.Payload.ToString());

            /*
            string a1 = unifiedReceipt.Payload.ToString();
            string a2 = a1.Replace(@"\r", @"").Replace(@"\n", @"").Replace(" ", "+");
            Debug.Log("t5="+a2);
            string[] strlist = a1.Split('/');
            string a3 = "";
            for (int i = 0; i < strlist.Length; i++) {
                a3 = a3 + strlist[i];
            }
            a3 = a3.Replace(" ", "+");
            Debug.Log("a5="+a3);
            */
        }

        //购买成功之后的回调
        string id = e.purchasedProduct.definition.id;
        //可以根据id进行发货操作
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        //Panel_Log.my.Error("没有购买成功！");
        Game_PublicClassVar.Get_game_PositionVar.OnPayResultReturnFail("支付失败");
        Debug.Log("没有购买成功");
        
    }
    public void OnPurchaseClicked(string productId)
    {
        Debug.Log("点击购买！" + productId);
        controller.InitiatePurchase(productId);
    }


    public void ClosePanel_Shop()
    {
        Debug.Log("关闭");
        gameObject.SetActive(false);
    }
}