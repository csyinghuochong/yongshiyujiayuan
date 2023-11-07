using UnityEngine;
using System.Collections;

public class UI_FightBtn : MonoBehaviour {

    //多点触碰检测
    private Vector2 DianJi_Vec2;        //点击的底坐标
    private Vector2 DianJiKuan_Vec2;    //点击的大小
    private Rect DianJiRect;            //点击区域
    public string Btn_Type;             //1:战斗  2：拾取

	// Use this for initialization
	void Start () {
        float x = this.GetComponent<RectTransform>().anchoredPosition.x-50;
        float y = this.GetComponent<RectTransform>().anchoredPosition.y-50;
        //Debug.Log("x = " + x);
        x = Game_PublicClassVar.Get_function_UI.ReturnScreen_X(x);
        y = Game_PublicClassVar.Get_function_UI.ReturnScreen_Y(y);
        DianJi_Vec2 = new Vector2(x, y);
        x = Game_PublicClassVar.Get_function_UI.ReturnScreen_X(100);
        y = Game_PublicClassVar.Get_function_UI.ReturnScreen_Y(100);
        DianJiKuan_Vec2 = new Vector2(x, y);
        //Debug.Log("格子" + SkillSpace + ":" + x + "," + y + "||" + "大小" + this.GetComponent<RectTransform>().localScale.x + "||" + "高宽" + this.GetComponent<RectTransform>().sizeDelta + "||触碰区域" + DianJiKuan_Vec2);
        DianJiRect = new Rect(DianJi_Vec2.x, DianJi_Vec2.y, DianJiKuan_Vec2.x, DianJiKuan_Vec2.y);
	}
	
	// Update is called once per frame
	void Update () {

        //检测多个触点
        GetTouchPosition(DianJiRect);

	}

    //当多个触电点击到技能ICON的区域
    public bool GetTouchPosition(Rect rect)
    {
        return false;
        int inputNum = Input.touchCount;
        //Debug.Log("我的手指数量：" + inputNum);
        for (int i = 0; i < inputNum; i++)
        {
            //检测触碰区域在右边平且触碰点大于等于2时
            if (Input.GetTouch(i).position.x > Screen.width / 2 && inputNum >= 2)
            {
                //Game_PublicClassVar.Get_function_UI.GameHint("rect = " + rect + ";" + Input.GetTouch(i).position.x + ";" + Input.GetTouch(i).position.y);

                if (rect.Contains(Input.GetTouch(i).position))
                {
                    //Game_PublicClassVar.Get_function_UI.GameHint("我触发了点击区域");
                    switch(Btn_Type){
                        case "1":
                            //触发攻击
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().AutomaticFight();
                            break;

                        case "2":
                            //触发拾取
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().AutomaticTake();
                            break;

                        case "3":
                            //触发翻滚
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().FanGunBtn();
                            break;

                        case "4":
                            //探索
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().TanSuoBtn();
                            break;

                        case "5":
                            //挂机
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().GuaJiBtn();
                            break;
                    }
                    

                    //this.GetComponent<RoseSkill_Sing_1>().ChuMoFingerId = i;
                }
            }
        }

        return true;
    }

}
