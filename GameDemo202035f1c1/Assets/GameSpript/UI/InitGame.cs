using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{


    public GameObject Obj_HuaWeiYinSi;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InitGame.Start");

        //打开隐私协议
        if (PlayerPrefs.GetString(YinSi.PlayerPrefsYinSi) != YinSi.YinSiValue)
        {
            Debug.Log("InitGame.YinSi false");
            Obj_HuaWeiYinSi.SetActive(true);
        }
        else
        {
            Debug.Log("InitGame.YinSi true");
            onRequestPermissionsResult("1_1");
        }
    }

    public int AgreeNumber = 0;
    //隐私权限
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void onRequestPermissionsResult(string permissons)
    {
        UnityEngine.Debug.Log($"onRecvPermissionsResult！ {permissons}");
        if (this.AgreeNumber >= 10)
        {
            return;
        }

        string[] values = permissons.Split('_');
        if (values[1] == "0")
        {
            //Application.Quit();
            //return;
            this.AgreeNumber = 10;
        }
        this.AgreeNumber++;
        if (this.AgreeNumber >= 2 || permissons == "1_1")
        {
            PlayerPrefs.SetString(YinSi.PlayerPrefsYinSi, YinSi.YinSiValue);
            UnityEngine.Debug.Log($"onRequestPermissionsResult: StartUpdate");

            //加载场景
            SceneManager.LoadScene("StartGame");        //加载场景
        }
        //弹出界面
        //Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().QingQiuQuanXianShow();
    }

    public void QuDaoRequestPermissions()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                UnityEngine.Debug.Log("unitycall: yyyy");
                jo.Call("QuDaoRequestPermissions" );
            }
        }
#else
        onRequestPermissionsResult("1_1");
#endif
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
