using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_ZhuLingShowList : MonoBehaviour
{

    public GameObject ParObj;
    public ObscuredString ShowZhuLingID;
    public GameObject ZhuLingIconShow;
    public GameObject ZhuLingNameShow;
    public GameObject ZhuLingXuanZhongImage;
    public GameObject Obj_ZhuLingNeedLv;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init() { 
    
        //鑾峰彇鏁版嵁
        string iconID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Iamge", "ID", ShowZhuLingID, "ZhuLing_Template");
        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ShowZhuLingID, "ZhuLing_Template");

        //灞曠ず
        object obj = ResourcesManager.Instance.LoadIconSync<Sprite>("OtherIcon/ZhuLingIcon/" + iconID);
        Sprite itemIcon = obj as Sprite;
        ZhuLingIconShow.GetComponent<Image>().sprite = itemIcon;
        ZhuLingNameShow.GetComponent<Text>().text = name;

        //鍒ゅ畾鑷韩鏄惁宸茬粡婵€娲?
        bool ifJiHuo = Game_PublicClassVar.Get_function_Rose.IfRoseZhuLing(ShowZhuLingID);
        if (ifJiHuo == false)
        {
            object huiObj = (Material)ResourcesManager.Instance.LoadEffectSync<Material>("UI_Effect/Sharde/UI_Hui");
            Material huiMaterial = huiObj as Material;
            ZhuLingIconShow.GetComponent<Image>().material = huiMaterial;
            //鏄剧ず绛夌骇
            string NeedRoseLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedLv", "ID", ShowZhuLingID, "ZhuLing_Template");
            Obj_ZhuLingNeedLv.GetComponent<Text>().text = "(婵€娲荤瓑绾?" + NeedRoseLvStr + ")";
        }
        else {
            //鏄剧ず绛夌骇
            Obj_ZhuLingNeedLv.GetComponent<Text>().text = "(宸叉縺娲?";
            Obj_ZhuLingNeedLv.GetComponent<Text>().color = new Color(0, 0.5f, 0);
            ZhuLingIconShow.GetComponent<Image>().material = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (ParObj != null) {
            if (ParObj.GetComponent<UI_ZhuLingSet>().NowZhuLingID == ShowZhuLingID)
            {
                ZhuLingXuanZhongImage.SetActive(true);
            }
            else {
                ZhuLingXuanZhongImage.SetActive(false);
            }

            if (ParObj.GetComponent<UI_ZhuLingSet>().UpdateZhuLingShowStatus) {
                Init();
            }
        }
        
    }



    public void Btn_XuanZe() {

        if (ParObj != null) {

            ParObj.GetComponent<UI_ZhuLingSet>().NowZhuLingID = ShowZhuLingID;
            ParObj.GetComponent<UI_ZhuLingSet>().Init();

        }

    }


}
