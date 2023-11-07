using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JianCeNpc : MonoBehaviour {

    public Dictionary<GameObject, float> NpcDisDic = new Dictionary<GameObject, float>();
    public int sumValue;
    public bool moveStatus;
    private float destoryTimeSum;
    public int JianCeType;
    public bool IfJianCeTask;

	// Use this for initialization
	void Start () {
        if (JianCeType == 0)
        {
            JianCeType = 9;
        }

        //Debug.LogError("检测到此Npc有任务！");
    }
	
	// Update is called once per frame
	void Update () {
        if (moveStatus)
        {
            sumValue = sumValue + 1;
            //第二帧
            GameObject nowNpcObj = null;
            if (sumValue == 2)
            {
                if (NpcDisDic.Count > 0)
                {
                    foreach (GameObject obj in NpcDisDic.Keys)
                    {
                        if (nowNpcObj == null)
                        {
                            nowNpcObj = obj;
                        }
                        else
                        {
                            //判断距离
                            float nowDis = NpcDisDic[obj];
                            if (nowDis < NpcDisDic[nowNpcObj])
                            {
                                nowNpcObj = obj;
                            }
                        }
                    }

                    if (nowNpcObj != null)
                    {
                        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                        if (roseStatus != null)
                        {
                            //检测点击Npc
                            if (JianCeType == 9)
                            {

                                //获取Npc当前是否有接取或者完成的任务
                                bool ifSpeakNpc = true;
                                if (IfJianCeTask)
                                {
                                    if (nowNpcObj.GetComponent<AI_NPC>().npcNameScri.Obj_TaskGet.activeSelf == true || nowNpcObj.GetComponent<AI_NPC>().npcNameScri.Obj_TaskComplete.activeSelf == true)
                                    {
                                        Debug.Log("检测到此Npc有任务！");
                                        ifSpeakNpc = true;
                                    }
                                    else
                                    {
                                        ifSpeakNpc = false;
                                    }
                                }

                                if (ifSpeakNpc)
                                {
                                    //取得鼠标点击的位置为释放技能的范围中心点
                                    roseStatus.Move_Target_Position = nowNpcObj.transform.position;

                                    //roseStatus.roseMoveDrop = false;
                                    roseStatus.roseMoveNpc = true;
                                    roseStatus.Move_Target_Status = true;
                                    //roseStatus.roseMoveGether = false;

                                    //设置点击的NPC
                                    roseStatus.obj_NpcTarget = nowNpcObj;
                                    //播放选中Npc特效
                                    if (nowNpcObj != null)
                                    {
                                        nowNpcObj.GetComponent<AI_NPC>().IfSeclectNpc = true;
                                    }

                                    //转身
                                    Vector3 v31 = new Vector3(nowNpcObj.transform.position.x, this.transform.position.y, nowNpcObj.transform.position.z);
                                    this.transform.LookAt(v31);
                                }
                            }

                            //检测点击采集物
                            if (JianCeType == 13)
                            {
                                //取得鼠标点击的位置为释放技能的范围中心点
                                roseStatus.Move_Target_Position = nowNpcObj.transform.position;
                                roseStatus.ui_GetherItem = nowNpcObj.GetComponent<UI_GetherItem>();
                                //开启移动状态，移动到目标处捡取物品
                                roseStatus.roseMoveGether = true;
                                //roseStatus.roseMoveDrop = false;
                                roseStatus.roseMoveNpc = false;
                                roseStatus.Move_Target_Status = true;

                                //转身
                                Vector3 v31 = new Vector3(nowNpcObj.transform.position.x, this.transform.position.y, nowNpcObj.transform.position.z);
                                this.transform.LookAt(v31);
                            }
                        }
                        Destroy(this.gameObject);
                    }
                }
            }
        }

        //1秒自动销毁
        destoryTimeSum = destoryTimeSum + Time.deltaTime;
        if (destoryTimeSum > 1) {
            Destroy(this.gameObject);
        }
    }

    //第一次碰撞调用
    
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("添加Npc！" + collider.name);
        GameObject colliderObj = collider.gameObject;
        if (colliderObj.layer == JianCeType) {
            //获取与目标的距离
            float dis = Vector3.Distance(colliderObj.transform.position,Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
            if (NpcDisDic.ContainsKey(colliderObj)) {
                NpcDisDic.Remove(colliderObj);
            }
            NpcDisDic.Add(colliderObj, dis);
            //Debug.Log("添加Npc！！！");
            moveStatus = true;
        }
    }

    /*

    //第一次碰撞调用
    void OnCollisionEnter(Collision collider)
    {
        Debug.Log("添加Npc！" + collider.gameObject.name);
        GameObject colliderObj = collider.gameObject;
        if (colliderObj.layer == 9)
        {
            //获取与目标的距离
            float dis = Vector3.Distance(colliderObj.transform.position, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
            if (NpcDisDic.ContainsKey(colliderObj))
            {
                NpcDisDic.Remove(colliderObj);
            }
            NpcDisDic.Add(colliderObj, dis);
            Debug.Log("添加Npc！！！");
            moveStatus = true;
        }
    }
    */
}
