using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ZhenYingPlayerXuanZe : MonoBehaviour
{
    public string ZhengYingSpace;
    public GameObject Obj_ZhenYingZhiWeiXuanZeList;
    public GameObject Obj_ZhenYingZhiWeiXuanZeListPar;

    // Start is called before the first frame update
    void Start()
    {
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ZhenYingZhiWeiXuanZeListPar);

        Pro_ZhenYingXuanZeDataList nowPro_ZhenYingXuanZeDataList = Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ProZhenYingXuanZeDataList;
        if (nowPro_ZhenYingXuanZeDataList.ProZhenYingRoseData.Count >= 1)
        {
            for (int i = 1; i <= nowPro_ZhenYingXuanZeDataList.ProZhenYingRoseData.Count; i++)
            {
                if (nowPro_ZhenYingXuanZeDataList.ProZhenYingRoseData.ContainsKey(i))
                {
                    GameObject obj = (GameObject)Instantiate(Obj_ZhenYingZhiWeiXuanZeList);
                    obj.transform.SetParent(Obj_ZhenYingZhiWeiXuanZeListPar.transform);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.GetComponent<UI_ZhenYingZhiWeiXuanZeList>().ZhengYingSpace = ZhengYingSpace;
                    obj.GetComponent<UI_ZhenYingZhiWeiXuanZeList>().ProZhenYingRoseData = nowPro_ZhenYingXuanZeDataList.ProZhenYingRoseData[i];
                    obj.GetComponent<UI_ZhenYingZhiWeiXuanZeList>().Obj_Par = this.gameObject;
                    obj.GetComponent<UI_ZhenYingZhiWeiXuanZeList>().Init();
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //πÿ±’”Œœ∑
    public void Btn_Close() {

        Destroy(this.gameObject);

    }
}
