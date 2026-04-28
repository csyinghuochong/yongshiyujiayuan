using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CameraAI : MonoBehaviour
{

    public GameObject FlowObj;                  //跟随移动的物体
    //private float cushionSpeed;               //缓冲移动速度
    //private RoseStatus roseObj;
    private bool storyStatus;                   //进入故事状态,镜头拉近
    private bool storyExitStatus;               //退出故事状态,镜头拉远
    public bool BuildEnterStatus;               //进入建筑状态
    public bool BuildExitStatus;                //退出建筑状态

    //private bool buildEnterEndStatus;           //进入建筑状态结束触发一次
    //private bool buildExitEndStatus;            //退出建筑状态结束触发一次

    public GameObject BuildMoveObj;             //移动到的建筑点
    private float cameraMoveTime;
    private float cameraMoveExitTime;
    private Rose_Status roseStatus;
    //public Transform EnterGameCameraPosition;
    private Quaternion exitCameraQuaternion;
    public Quaternion First_CameraQuaternion;   //第一次进入摄像机方向
    public Vector3 First_CameraVec3;            //第一次进入摄像机位置

    public List<GameObject> HideObjList = new List<GameObject>();       //摄像机遮挡隐藏物体列表
    private bool roseHideStatus;
    private GameObject[] RoseObjList;

    // Use this for initialization
    void Start()
    {
        roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        //EnterGameCameraPosition = new Vector3(-5f, 40f, -21f);
        //EnterGameCameraPosition = Camera.main.transform;

        switch (Game_PublicClassVar.Get_wwwSet.RoseID) {
            //战士
            case "10001":
                RoseObjList = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().ZhanShi_ModelShowShader;
                break;

            //法师 后期补偿
            case "10002":
                RoseObjList = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().FaShi_ModelShowShader;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //获取当前角色是否进入故事模式
        storyStatus = roseStatus.RoseStorySpeakStatus;
        //故事模式,镜头移动
        if (storyStatus)
        {
            cameraMoveTime = cameraMoveTime + Time.deltaTime*2;
            if (cameraMoveTime <= 1)
            {
                transform.localPosition = new Vector3(FlowObj.transform.position.x, 37.0f - 5 * cameraMoveTime, FlowObj.transform.position.z - 7f + 2 * cameraMoveTime);
                transform.localRotation = Quaternion.Euler(new Vector3(45 - 30 * cameraMoveTime, 0, 0));
                storyExitStatus = true;
                //清空镜头移动的计数器
                cameraMoveExitTime = 0;
            }
        }
        else {
            //退出故事模式,镜头移动
            if (storyExitStatus)
            {
                cameraMoveExitTime = cameraMoveExitTime + Time.deltaTime*2;
                if (cameraMoveExitTime <= 1)
                {
                    transform.localPosition = new Vector3(FlowObj.transform.position.x, 32.0f + 5 * cameraMoveExitTime, FlowObj.transform.position.z - 5f - 2 * cameraMoveExitTime);
                    transform.localRotation = Quaternion.Euler(new Vector3(15 + 30 * cameraMoveExitTime, 0, 0));
                    //storyExitStatus = true;
                    //清空镜头移动的计数器
                    cameraMoveTime = 0;
                }
                else {
                    storyExitStatus = false;
                }
            }
            else {
                //普通进入游戏摄像机状态
                if (Game_PublicClassVar.Get_game_PositionVar.EnterRoseCameraDrawStatus)
                {
                    //建筑模式,镜头移动
                    if (BuildEnterStatus)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ai_nav.speed = 0;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ai_nav.SetDestination(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);


                        //触发一次
                        if (cameraMoveTime == 0)
                        {
                            //隐藏角色模型
                            Camera.main.cullingMask &= ~(1 << 14);
                            //隐藏UI
                            UI_Set ui_Set = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>();
                            ui_Set.Obj_UI_AIHpSet.SetActive(false);
                            ui_Set.Obj_RoseTask.SetActive(false);
                            ui_Set.Obj_UIMapName.SetActive(false);
                            ui_Set.Obj_MainUI.SetActive(false);
                            ui_Set.Obj_NpcNameSet.SetActive(false);
                            ui_Set.Obj_BuildingHuoBiSet.SetActive(false);
                        }
                        cameraMoveTime = cameraMoveTime + Time.deltaTime * 2;
                        if (cameraMoveTime <= 1)
                        {
                            Vector3 chaV3 = BuildMoveObj.transform.position - this.transform.position;
                            float pos_x = this.transform.position.x + chaV3.x * cameraMoveTime;
                            float pos_y = this.transform.position.y + chaV3.y * cameraMoveTime;
                            float pos_z = this.transform.position.z + chaV3.z * cameraMoveTime;
                            transform.localPosition = new Vector3(pos_x, pos_y, pos_z);
                            
                            float qua_x = this.transform.rotation.eulerAngles.x + (BuildMoveObj.transform.rotation.eulerAngles.x - this.transform.rotation.eulerAngles.x) * cameraMoveTime;
                            float qua_y = 0;
                            if (BuildMoveObj.transform.rotation.eulerAngles.y <= 180)
                            {
                                qua_y = this.transform.rotation.eulerAngles.y + (BuildMoveObj.transform.rotation.eulerAngles.y - this.transform.rotation.eulerAngles.y) * cameraMoveTime;
                            }
                            else {
                                //因为其他摄像机不是均速移动所以在此处加0.2秒,预估与其他坐标轴保持一致
                                float yyy = cameraMoveTime+0.2f;
                                if (yyy >= 1) {
                                    yyy = 1;
                                }
                                qua_y = 360 + (BuildMoveObj.transform.rotation.eulerAngles.y - 360) * yyy;
                            }

                            float qua_z = this.transform.rotation.eulerAngles.z + (BuildMoveObj.transform.rotation.eulerAngles.z - this.transform.rotation.eulerAngles.z) * cameraMoveTime;

                            transform.rotation = Quaternion.Euler(qua_x, qua_y, qua_z);
                            //qua_y = BuildMoveObj.transform.rotation.eulerAngles.y;
                            //Debug.Log("qua_x = " + qua_x + ";BuildMoveObj.transform.rotation.eulerAngles.x = " + BuildMoveObj.transform.rotation.eulerAngles.x);
                            //Debug.Log("qua_y = " + qua_y + ";BuildMoveObj.transform.rotation.eulerAngles.y = " + BuildMoveObj.transform.rotation.eulerAngles.y);
                            //Debug.Log("qua_z = " + qua_z + ";BuildMoveObj.transform.rotation.eulerAngles.z = " + BuildMoveObj.transform.rotation.eulerAngles.z);
                            //BuildMoveObj.transform.localRotation.eulerAngles

                            /*
                            float roa_x = this.transform.rotation.x + (BuildMoveObj.transform.rotation.x - this.transform.rotation.x) * cameraMoveTime;
                            float roa_y = this.transform.rotation.y + (BuildMoveObj.transform.rotation.y - this.transform.rotation.y) * 1;
                            float roa_z = this.transform.rotation.z + (BuildMoveObj.transform.rotation.z - this.transform.rotation.z) * cameraMoveTime;
                            float roa_w = this.transform.rotation.w + (BuildMoveObj.transform.rotation.w - this.transform.rotation.w) * cameraMoveTime;
                            transform.localRotation = new Quaternion(roa_x, roa_y, roa_z, roa_w);
                            */
                            /*
                            Vector3 chaV3 = BuildMoveObj.transform.position - First_CameraVec3;
                            float pos_x = First_CameraVec3.x + chaV3.x * cameraMoveTime;
                            float pos_y = First_CameraVec3.y + chaV3.y * cameraMoveTime;
                            float pos_z = First_CameraVec3.z + chaV3.z * cameraMoveTime;
                            transform.localPosition = new Vector3(pos_x, pos_y, pos_z);
                            float roa_x = First_CameraQuaternion.x + (BuildMoveObj.transform.rotation.x - First_CameraQuaternion.x) * cameraMoveTime;
                            float roa_y = First_CameraQuaternion.y + (BuildMoveObj.transform.rotation.y - First_CameraQuaternion.y) * cameraMoveTime;
                            float roa_z = First_CameraQuaternion.z + (BuildMoveObj.transform.rotation.z - First_CameraQuaternion.z) * cameraMoveTime;
                            float roa_w = First_CameraQuaternion.w + (BuildMoveObj.transform.rotation.w - First_CameraQuaternion.w) * cameraMoveTime;
                            transform.localRotation = new Quaternion(roa_x, roa_y, roa_z, roa_w);
                            */
                            exitCameraQuaternion = transform.localRotation;
                        }
                    }
                    else {
                        if (BuildExitStatus)
                        {
                            //触发一次
                            if (cameraMoveExitTime == 0)
                            {
                                //显示角色模型
                                Camera.main.cullingMask |= (1 << 14);
                                //隐藏UI
                                UI_Set ui_Set = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>();
                                ui_Set.Obj_UI_AIHpSet.SetActive(true);
                                ui_Set.Obj_RoseTask.SetActive(true);
                                ui_Set.Obj_UIMapName.SetActive(true);
                                ui_Set.Obj_NpcNameSet.SetActive(true);
                                //ui_Set.Obj_BuildingHuoBiSet.SetActive(true);
                                if (SceneManager.GetActiveScene().name != "EnterGame") {
                                    ui_Set.Obj_MainUI.SetActive(true);
                                }
                            }
                            cameraMoveExitTime = cameraMoveExitTime + Time.deltaTime * 2;
                            if (cameraMoveExitTime <= 1)
                            {
                                //Debug.Log("First_CameraQuaternion = " + First_CameraQuaternion);
                                Vector3 chaV3 = BuildMoveObj.transform.position - First_CameraVec3;
                                transform.localPosition = new Vector3(BuildMoveObj.transform.position.x - chaV3.x * cameraMoveExitTime, BuildMoveObj.transform.position.y - chaV3.y * cameraMoveExitTime, BuildMoveObj.transform.position.z - chaV3.z * cameraMoveExitTime);
                                //transform.localRotation = new Quaternion(exitCameraQuaternion.x - (BuildMoveObj.transform.rotation.x - First_CameraQuaternion.x) * cameraMoveExitTime, exitCameraQuaternion.y - (BuildMoveObj.transform.rotation.y - First_CameraQuaternion.y) * cameraMoveExitTime, exitCameraQuaternion.z - (BuildMoveObj.transform.rotation.z - First_CameraQuaternion.z) * cameraMoveExitTime, exitCameraQuaternion.w - (BuildMoveObj.transform.rotation.w - First_CameraQuaternion.w) * cameraMoveExitTime);
                                float qua_x = exitCameraQuaternion.eulerAngles.x - (BuildMoveObj.transform.rotation.eulerAngles.x - First_CameraQuaternion.eulerAngles.x) * cameraMoveExitTime;
                                float qua_y = 0;
                                if (BuildMoveObj.transform.rotation.eulerAngles.y <= 180)
                                {
                                    qua_y = exitCameraQuaternion.eulerAngles.y - (BuildMoveObj.transform.rotation.eulerAngles.y - First_CameraQuaternion.eulerAngles.y) * cameraMoveExitTime;
                                }
                                else
                                {
                                    //因为其他摄像机不是均速移动所以在此处加0.2秒,预估与其他坐标轴保持一致
                                    float yyy = cameraMoveExitTime + 0.0f;
                                    qua_y = 360 + (360 - BuildMoveObj.transform.rotation.eulerAngles.y) * yyy;
                                }

                                float qua_z = exitCameraQuaternion.eulerAngles.z - (BuildMoveObj.transform.rotation.eulerAngles.z - First_CameraQuaternion.eulerAngles.z) * cameraMoveExitTime;

                                transform.localRotation = Quaternion.Euler(qua_x, qua_y, qua_z);

                                //Debug.Log("qua_x = " + qua_x + ";BuildMoveObj.transform.rotation.eulerAngles.x = " + First_CameraQuaternion.eulerAngles.x);
                                //Debug.Log("qua_y = " + qua_y + ";BuildMoveObj.transform.rotation.eulerAngles.y = " + First_CameraQuaternion.eulerAngles.y);
                                //Debug.Log("qua_z = " + qua_z + ";BuildMoveObj.transform.rotation.eulerAngles.z = " + First_CameraQuaternion.eulerAngles.z);
                            
                            }
                            else {
                                BuildExitStatus = false;
                                BuildEnterStatus = false;
                                cameraMoveTime = 0;
                                cameraMoveExitTime = 0;
                                Game_PublicClassVar.Get_game_PositionVar.EnterRoseCameraDrawStatus = false;
                                //Debug.Log("值清空");
                            }
                        }

                        else {
                            //transform.localPosition = EnterGameCameraPosition.position;
                            //transform.localRotation = Quaternion.Euler(new Vector3(45, 45, 0));
                        }
                    }
                }
                else
                {
                    //进入实际游戏界面
                    transform.localPosition = new Vector3(FlowObj.transform.position.x, FlowObj.transform.position.y+9.0f, FlowObj.transform.position.z - 7f);
                    transform.localRotation = Quaternion.Euler(new Vector3(45, 0, 0));
                }
            }
        }



        //隐藏遮挡主角的物体
        RaycastHit hit;
        if (Physics.Linecast(this.transform.position, FlowObj.transform.position, out hit))
        {
            GameObject obj = hit.collider.gameObject;
            string name = obj.tag;
            //if (name != "MainCamera" && name != "terrain" && obj.name != "Rose" && obj.layer != 9)
            if (obj.layer == 11|| obj.layer == 17)
            {
                //Debug.Log("摄像机碰撞:" + obj.name);
                /*
                //隐藏遮挡的物体
                if (obj.GetComponent<MeshRenderer>() != null) {
                    if (obj.GetComponent<MeshRenderer>().enabled == true) {
                        obj.GetComponent<MeshRenderer>().enabled = false;
                        HideObjList.Add(obj);
                    }
                }
                */

                if (!roseHideStatus) {

                    for (int i = 0; i < RoseObjList.Length; i++)
                    {
                        GameObject nowObj = RoseObjList[i];
                        nowObj.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_RimMode", 0.0f);
                    }

                    roseHideStatus = true;
                }

            }
            else {

                //显示遮挡的物体
                /*
                if (HideObjList.Count >= 1) {
                    
                    foreach (GameObject nowObj in HideObjList) {
                        if (nowObj != null) {
                            if (nowObj.GetComponent<MeshRenderer>() != null)
                            {
                                nowObj.GetComponent<MeshRenderer>().enabled = true;
                            }
                        }
                    }
                    HideObjList.Clear();
                }
                */

                if (roseHideStatus) {
                    roseHideStatus = false;
                    //Debug.Log("显示mODEL");
                    for (int i = 0; i < RoseObjList.Length; i++)
                    {
                        GameObject nowObj = RoseObjList[i];
                        nowObj.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_RimMode", 1.0f);
                    }
                }
            }
        }
    }
}
