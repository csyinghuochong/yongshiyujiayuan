using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class PastureKuangSet : MonoBehaviour
{

    public ObscuredString KuangID;
    public ObscuredString KuangSpaceID;
    public GameObject Obj_Position;
    public GameObject Obj_ShowTitle;
    public GameObject ShowTitleObj;
    public bool WeiKaiFaStatus;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    //��ʼ��
    public void Init() {

        //Debug.Log("��ʼ��kuang...");

        if(ShowTitleObj!=null) {
            Destroy(ShowTitleObj);
        }

        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Obj_Position.transform.position);
        Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

        //ʵ����UI
        ShowTitleObj = (GameObject)Instantiate(Obj_ShowTitle);

        //��ʾUI,��������Ӧ����������
        ShowTitleObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_PastureKuangSet.transform);
        ShowTitleObj.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        ShowTitleObj.transform.localScale = new Vector3(1f, 1f, 1f);
        //ShowTitleObj.GetComponent<UI_PastureKuangShowTitle>().Obj_KuangShow.GetComponent<Text>().text = Game_PublicClassVar.function_Pasture.PastureKuang_GetTypeName(KuangID);
        //Debug.Log("KuangSpaceID = " + KuangSpaceID);
        ShowTitleObj.GetComponent<UI_PastureKuangShowTitle>().KuangSpaceID = KuangSpaceID;
        ShowTitleObj.GetComponent<UI_PastureKuangShowTitle>().WeiKaiFaStatus = WeiKaiFaStatus;
        ShowTitleObj.GetComponent<UI_PastureKuangShowTitle>().Init();

    }


    // Update is called once per frame
    void Update()
    {

        //��������Ѫ��λ��
        if (ShowTitleObj != null && Obj_Position !=null)
        {

            Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Obj_Position.transform.position);
            Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

            //Ѫ��λ�����������ݷֱ��ʵı仯���仯��
            ShowTitleObj.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        }

    }

    public void OnDestroy()
    {
        Destroy(ShowTitleObj);
    }


    //��ҵ���¼�
    public void Click() {

        if (WeiKaiFaStatus == false)
        {
            GameObject showobj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_PastureKuangShowName);
            showobj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            showobj.transform.localScale = new Vector3(1, 1, 1);
            showobj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            showobj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            showobj.GetComponent<UI_PastureKuangShowName>().KuangSpaceID = KuangSpaceID;
            showobj.GetComponent<UI_PastureKuangShowName>().Init();     //��ʼ��
        }
    }
}
