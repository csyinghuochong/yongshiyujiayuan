using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using Newtonsoft.Json;
using System;

public class GooglePay : MonoBehaviour, IStoreListener
{

    private IStoreController mStoreController;
    private IExtensionProvider mExtensionProvider;
    ConfigurationBuilder builder;

    //private string consumeable = "product1";

    //private string consumeable = "android.test.purchased";
    //private string consumeable = "paytest1";
    private string consumeable = "android.test.purchased";
    private string consumeable_1 = "paytest3";
    private string consumeable_2 = "paytest2";

    //public string NowPayStr;

    void Start()
    {

    //如果是IOS则卸载脚本
#if UNITY_IPHONE
        Destroy(this);
#endif

        if (mStoreController == null)
        {
            InitPurchasing();
        }
    }


    //初始化
    public void InitPurchasing()
    {
#if UNITY_ANDROID

        Debug.Log("开始初始化支付...");
        if (IsInitialized())
        {
            return;
        }

        builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //builder.AddProduct(consumeable, ProductType.NonConsumable);

        builder.AddProduct(consumeable, ProductType.Consumable);
        builder.AddProduct(consumeable_1, ProductType.Consumable);
        builder.AddProduct(consumeable_2, ProductType.Consumable);

        builder.AddProduct("pay_1", ProductType.Consumable);
        builder.AddProduct("pay_2", ProductType.Consumable);
        builder.AddProduct("pay_3", ProductType.Consumable);
        builder.AddProduct("pay_4", ProductType.Consumable);
        builder.AddProduct("pay_5", ProductType.Consumable);
        builder.AddProduct("pay_6", ProductType.Consumable);
        builder.AddProduct("pay_7", ProductType.Consumable);
        builder.AddProduct("pay_8", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
#endif
    }


    //购买
    public void BuyProduct(string payStr)
    {
        try
        {
            //payStr = "paytest3";
            Debug.Log("点击购买道具链接..." + payStr);
            if (IsInitialized())
            {
                Debug.Log("点击购买道具链接22222...");
                Product produdt = mStoreController.products.WithID(payStr);
                if (produdt != null && produdt.availableToPurchase)
                {
                    Debug.Log("点击购买道具链接33333...");
                    mStoreController.InitiatePurchase(produdt);
                    Debug.Log("ssss = " + produdt.metadata.localizedPrice);
                    Debug.Log("发送购买消息...");
                }
                else
                {
                    Debug.Log("fail");
                }
            }
            else
            {
                Debug.Log("没有进来购买链接,直接跳出！");
            }
        }
        catch(Exception ex) {
            Debug.Log("谷歌支付报错:" + ex);
        }

    }


    //购买实际效果
    public void BuyProduct_Test()
    {
        Debug.Log("点击购买道具链接...");
        if (IsInitialized())
        {
            Debug.Log("点击购买道具链接22222...");
            Product produdt = mStoreController.products.WithID(consumeable_1);
            if (produdt != null && produdt.availableToPurchase)
            {
                Debug.Log("点击购买道具链接33333...");
                mStoreController.InitiatePurchase(produdt);
                Debug.Log("ssss = " + produdt.metadata.localizedPrice);
                Debug.Log("发送购买消息...");
            }
            else
            {
                Debug.Log("fail");
            }
        }
        else
        {
            Debug.Log("没有进来购买链接,直接跳出！");
        }
    }


    //购买实际效果
    public void BuyProduct_Test_2()
    {
        Debug.Log("点击购买道具链接...");
        if (IsInitialized())
        {
            Debug.Log("点击购买道具链接22222...");
            Product produdt = mStoreController.products.WithID(consumeable_2);
            if (produdt != null && produdt.availableToPurchase)
            {
                Debug.Log("点击购买道具链接33333...");
                mStoreController.InitiatePurchase(produdt);
                Debug.Log("ssss = " + produdt.metadata.localizedPrice);
                Debug.Log("发送购买消息...");
            }
            else
            {
                Debug.Log("fail");
            }
        }
        else
        {
            Debug.Log("没有进来购买链接,直接跳出！");
        }
    }



    //恢复购买
    public void ReSotre()
    {
        if (!IsInitialized())
        {
            return;
        }

        if (mExtensionProvider != null
            && (Application.platform == RuntimePlatform.IPhonePlayer
                || Application.platform == RuntimePlatform.OSXPlayer))
        {
            var apple = mExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                // Restore purchases initiated. See ProcessPurchase for any restored transacitons.
            });
        }
    }

    private bool IsInitialized()
    {
        return mStoreController != null && mExtensionProvider != null;
    }


    //---------------IStoreListener的四个接口的实现-----------

    //初始化成功
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("初始化变量值...." + controller.products.all);
        mStoreController = controller;
        mExtensionProvider = extensions;
    }

    //初始化失败
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("初始化失败了：" + error);
    }

    //购买失败
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log("支付失败...");
        Debug.Log(i.definition.id + "\n" + p);
    }


    //购买成功和恢复成功的回调，可以根据id的不同进行不同的操作
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        //if (string.Equals(e.purchasedProduct.definition.id, consumeable, System.StringComparison.Ordinal))
        //{
        //获取商品的描述信息，标题，价格，带单位的（￥，$等等）
        //这一步一般可以放在初始化完成的回调里，用于刷新你的相关UI
        Debug.Log("price:" + e.purchasedProduct.metadata.localizedPriceString);
        Debug.Log(e.purchasedProduct.metadata.localizedTitle + e.purchasedProduct.metadata.localizedDescription);       //打印说明

        //交易号
        Debug.Log("storeSpecificId:" + e.purchasedProduct.definition.storeSpecificId);          //pay_1

        //回执单  
        Debug.Log("receipt:" + e.purchasedProduct.receipt);

        //商品的id号
        Debug.Log("transactionID:" + e.purchasedProduct.transactionID);     //这里是商品的id号

        Debug.Log("购买成功了！！！！");

        //请求服务器进行效验
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //参数：协议号,账号ID,平台ID,交易金额,交易状态,google订单id,google回执验证码
        string huizhiStr = e.purchasedProduct.receipt;
        int start = huizhiStr.IndexOf("purchaseToken") + 22;
        int end = huizhiStr.IndexOf("signature") - 10;

        Debug.Log("start = " + start + "end = " + end);

        string tokenStr = huizhiStr.Substring(start, end - start);


        //测试解析
        //string xinxi = e.purchasedProduct.receipt;
        //Receipt_Google rt = JsonConvert.DeserializeObject<Receipt_Google>(xinxi);
        //Debug.Log("rt.purchaseToken = " + rt.purchaseToken);


        string sendStr = "GooglePay," + zhanghaoID + "," + "4" + "," + "0"+ "," + "1" + "," + e.purchasedProduct.definition.storeSpecificId + "," + tokenStr;
        Debug.Log("sendStr = " + sendStr);
        this.GetComponent<GamePayLinkServer>().SendToServer(sendStr);

        /*

        //这里是获取订单号，用于存储到自己的服务器，以及恢复购买时的对比
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
        // Get a reference to IAppleConfiguration during IAP initialization.
        var appleConfig = builder.Configure<IAppleConfiguration>();
        var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
        AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

        Debug.Log(receipt.bundleID);
        Debug.Log(receipt.receiptCreationDate);
        foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts)
        {
            Debug.Log("订单号：" + productReceipt.originalTransactionIdentifier);
        }
#endif
*/
        //}
        return PurchaseProcessingResult.Complete;

    }


    public class Receipt_Google
    {
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        public string purchaseToken { get; set; }

    }
}
