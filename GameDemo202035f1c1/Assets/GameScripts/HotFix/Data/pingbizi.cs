using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;


public class pingbizi : MonoBehaviour {

    string[] SentiWords = null;//定义一个接受文件内容的字符串数组

    // Use this for initialization
    void Start () {
        //Debug.Log("开始加载屏蔽字");
        LoadWWW().Forget();
        //添加输入事件监听
        transform.GetComponent<InputField>().onValueChanged.AddListener(OnValueChanged);

    }



	
	// Update is called once per frame
	void Update () {
		
	}

 



    /// <summary>
    /// 监听方法，该方法会在监测到输入值改变时被触发
    /// </summary>
    /// <param name="t"></param> 参数为当前输入的值
    public void OnValueChanged(string t)
    {
        if (SentiWords == null)
            return;
        foreach (string ssr in SentiWords)
        {
            if (t.Contains(ssr))
            {
                if (!ssr.Equals(""))
                {
                    //Debug.Log("包含敏感词汇:" + ssr + ",需要进行替换");
                    string stt = transform.GetComponent<InputField>().text;
                    int length = ssr.ToCharArray().Length;
                    string s = "";
                    for (int i = 0; i < length; i++)
                        s += "*";
                    //Debug.Log(stt.Replace(ssr, s));
                    stt = stt.Replace(ssr, s);
                    transform.GetComponent<InputField>().text = stt;
                }
            }
            //            Debug.Log(ssr);
        }
    }


    /// <summary>
    /// 使用一个协程来进行文件读取
    /// </summary>
    /// <returns></returns>
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    private async UniTask LoadWWW()
    {
        var text = await ResourcesManager.Instance.LoadAssetAsync<TextAsset>(ABPathHelper.GetTextPath("SensitiveWords1"));
        
        if (!(text.text.Equals("") || text.text.Equals(null)))
        {
            //Debug.Log("开始加载屏蔽字33333");
            //Debug.Log(www.text);
            //将读取到的字符串进行分割后存储到定义好的数组中
            SentiWords = text.text.Split('、');
        }
    }

}
