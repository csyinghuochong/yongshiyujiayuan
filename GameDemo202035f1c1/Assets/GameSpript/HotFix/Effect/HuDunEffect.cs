using UnityEngine;
using System.Collections;
using System;

public class HuDunEffect : MonoBehaviour {

    private Vector3 rosePosiVec3;
    private Rose_Bone roseBone;
    private Rose_Proprety roseProprety;
	// Use this for initialization
	void Start () {

        this.gameObject.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.parent);
        roseBone = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>();
        roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

        //护盾特效 过地图 不消失
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {

        try
        {
            //始终跟随这Rose进行移动
            if (roseBone != null)
            {
                rosePosiVec3 = roseBone.Bone_Center.transform.position;
                this.transform.position = rosePosiVec3;
                if (roseProprety != null)
                {
                    if (roseProprety.HuDunStatus == false)
                    {
                        if (this.gameObject != null)
                        {
                            Destroy(this.gameObject);
                        }
                    }
                }
            }
            else
            {
                if (this.gameObject != null)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        catch (Exception ex) {
            Debug.LogError("护盾特效报错HuDunEffect:" + ex);
        }

	}

    //注销时调用
    private void OnDestroy()
    {
        //英勇护盾销毁时需要销毁自身的buff
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null)
        {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "1")
            {

                Buff_4[] buff_4 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_4>();
                for (int i = 0; i <= buff_4.Length - 1; i++)
                {
                    if (buff_4[i].BuffID == "90000055")
                    {
                        //Destroy(buff_4[i]);
                    }
                    if (buff_4[i].BuffID == "90000056")
                    {
                        //Destroy(buff_4[i]);
                    }
                }
            }
        }
    }
}
