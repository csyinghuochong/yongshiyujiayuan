using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;

/// <summary>
/// 这是通用方式，通过读取catalog里面的信息，获取所有商品信息
/// </summary>
public class ShopList : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    bool blnRestore = false;//用来表示
    bool blnPressRestore = false;//用来区分是否按了	restore 按钮

    int dataLen = 3;
    //每个商品的内容（金币），价格（除以100），折扣
    int[] shopData = new int[]{
        150,99,0,	//0.99 美元购买 150 个金币
		450,299,0,	//2.99 美元购买 450 个金币
		850,499,12,	//4.99 美元购买 850 个金币
		1850,999,22,	//9.99 美元购买 1850 个金币
		3950,1999,30,	//19.99 美元购买 3950 个金币
		0,199,0		//1.99 美元购买去广告功能，可以Restore的项目
	};

    //catalog 里面设置的id，和这边一一对应，这个id是unity端的一个映射，在catalog里面可以对应不同平台的真实的 商品id
    private string[] kProducts = new string[] {
        "iospay6",
        "iospay30",
        "iospay50",
        "iospay98",
        "iospay198",
        "iospay298",
        "iospay488",
        "iospay648"
    };
    void Start()
    {
        InitializePurchasing();
    }
    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    //初始化内购项目，主要是从catalog中获取商品信息，设置给 UnityPurchasing
    void InitializePurchasing()
    {
        if (IsInitialized())
        {
            Debug.Log("初始化失败");
            return;
        }
        StandardPurchasingModule module = StandardPurchasingModule.Instance();
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
        //通过编辑器中的Catalog添加，方便操作
        ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
        // Debug.Log(catalog.allProducts.Count);
        foreach (var product in catalog.allProducts)
        {
            if (product.allStoreIDs.Count > 0)
            {
                // Debug.Log("product:" + product.id);
                var ids = new IDs();
                foreach (var storeID in product.allStoreIDs)
                {
                    ids.Add(storeID.id, storeID.store);
                    // Debug.Log("stordId:" + storeID.id  + ", " + storeID.store);
                }
                builder.AddProduct(product.id, product.type, ids);
            }
            else
            {
                builder.AddProduct(product.id, product.type);
            }
        }
        UnityPurchasing.Initialize(this, builder);
    }

    //供外部调用，当按 Restore 按钮时触发
    public void OnRestore()
    {
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Restore success!");
        }
        else
        {
            blnRestore = true;
            RestorePurchases();
            blnPressRestore = true;
        }
    }
    //供外部调用，按下哪个按钮，就可以购买哪一档的金币，我这里是通过按钮的名称得到购买的 idx 的，可以根据自己需要更改，比如：OnBuyCoins(int idx)
    //idx 是上面 shopData 对应的每行数据
    public void OnBuyCoins(int idx)
    {
        //int idx = System.Convert.ToInt32(btn.name);
        BuyCoinsWithIdx(idx);
    }
    //实际购买调用的函数，根据idx拿到unity端的商品id
    void BuyCoinsWithIdx(int idx)
    {
        /*
        if (idx == 5)
        {//购买去广告
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.Log("购买去广告!");
            }
            else
            {
                blnPressRestore = false;
                BuyProductID(kProducts[idx]);
            }
        }
        else
        {
        }
        */
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("editor buy coins");
        }
        else
        {
            //调用购买信息
            BuyProductID(kProducts[idx]);
        }
        
    }
    //这里是通过商品id购买物品
    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Debug.Log("Buy ProductID: " + productId);
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("没初始化");
        }
    }
    //真是的发起Restore请求
    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("没初始化");
            return;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions(HandleRestored);
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
    //如果restore之后，会返回一个状态，如果状态为true，那边以前购买的非消耗物品都会回调一次 ProcessPurchase 然后在这里个回调里面进行处理
    void HandleRestored(bool result)
    {
        //返回一个bool值，如果成功，则会多次调用支付回调，然后根据支付回调中的参数得到商品id，最后做处理(ProcessPurchase)
        Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
        blnRestore = false;
        if (result)
        {
            Debug.Log("Restore success3333!");
        }
        else
        {
            Debug.Log("Restore Failed33333!");
        }
    }

    //初始化回调
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //初始化成功
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //初始化失败
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        if (error == InitializationFailureReason.AppNotKnown)
        {
            //
        }
        else if (error == InitializationFailureReason.NoProductsAvailable)
        {
            //
        }
        else if (error == InitializationFailureReason.PurchasingUnavailable)
        {
            //
        }
    }
    //购买成功后的回调，包括restore的商品
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));


        /*
        //根据不同的id，做对应的处理。。
        int key = -1;
        for (int i = 0; i < kProducts.Length; i++)
        {
            if (string.Equals(args.purchasedProduct.definition.id, kProducts[i], System.StringComparison.Ordinal))
            {
                key = i;
                break;
            }
        }
        if (key == -1)
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
        else
        {
            if (key == 5)
            {
                if (!blnPressRestore)
                {
                    Debug.Log("Ads have been removed!");
                }
            }
            else
            {
                Debug.Log("购买了" + shopData[key * dataLen].ToString() + "个金币");
            }
        }
        */

        Debug.Log("aaa:" + args.purchasedProduct.metadata.localizedTitle + ";" + args.purchasedProduct.metadata.localizedDescription);
        Debug.Log("订单号1:" + args.purchasedProduct.receipt);
        Debug.Log("transaiD:" + args.purchasedProduct.transactionID);
        Debug.Log("storeid:" + args.purchasedProduct.definition.storeSpecificId);
        Debug.Log("price:" + args.purchasedProduct.metadata.localizedPriceString);
        //args.purchasedProduct.receipt.

        var unifiedReceipt = JsonUtility.FromJson<UnifiedReceipt>(args.purchasedProduct.receipt);

        if (unifiedReceipt != null && !string.IsNullOrEmpty(unifiedReceipt.Payload))
        {
            //调用触发
            bool ifsave = true;

            try
            {
                //读取json的验证信息
                Debug.Log("验证票据:" + unifiedReceipt.Payload.ToString());

                if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set != null && Game_PublicClassVar.Get_wwwSet.DataUpdataStatus && Application.loadedLevelName != "StartGame")
                {
                    if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore != null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.OnPayResult(unifiedReceipt.Payload.ToString());
                        ifsave = false;
                    }
                }

            }
            catch (Exception Ex) {

                Debug.LogError("IOS支付报错：" + Ex);
                ifsave = true;
            }


            //存储交易票据
            if (ifsave == true) {
                Game_PublicClassVar.Get_game_PositionVar.SaveIosPay(unifiedReceipt.Payload.ToString());
            }

        }

        return PurchaseProcessingResult.Complete;       //如果屏蔽此处,程序每隔一段时间会返回一次调用,表示没有完成一次支付,每次重启也会自动调用一次
    }
    //购买失败回调，根据具体情况给出具体的提示
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //支付失败
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        if (failureReason == PurchaseFailureReason.UserCancelled)
        {
            //用户取消交易
        }
        else if (failureReason == PurchaseFailureReason.ExistingPurchasePending)
        {
            //上一笔交易还未完成
        }
        else if (failureReason == PurchaseFailureReason.PaymentDeclined)
        {
            //拒绝付款
        }
        else if (failureReason == PurchaseFailureReason.ProductUnavailable)
        {
            //商品不可用
        }
        else if (failureReason == PurchaseFailureReason.PurchasingUnavailable)
        {
            //支付不可用
        }
        else
        {
            //位置错误
        }

        Debug.Log("用户支付取消");
        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set != null && Game_PublicClassVar.Get_wwwSet.DataUpdataStatus && Application.loadedLevelName != "StartGame")
        {
            if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore != null)
            {
                Game_PublicClassVar.Get_game_PositionVar.OnPayResultReturnFail("");
            }
        }

        
    }
}