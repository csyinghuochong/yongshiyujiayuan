using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoseSkillZhiShi : MonoBehaviour
{
    private enum SKillAreaElement
    {
        OuterCircle,    // 外圆
        InnerCircle,    // 内圆
        Cube,           // 矩形 
        Sector60,        // 扇形
        Sector120,        // 扇形
    }

    private float outerRadius = 3f;      // 外圆半径
    private float innerRadius = 1f;     // 内圆半径
    private float cubeWidth = 2f;       // 矩形宽度 （矩形长度使用的外圆半径）
    private int angle = 0;             // 扇形角度

    public GameObject RoseObj;
    public Transform SkillGameObj;
    public GameObject Skill_Area;
    public GameObject Skill_Dir;
    public GameObject Skill_Area_60;
    public GameObject Skill_Area_120;
    public GameObject Skill_InnerArea;

    //public GameObject ZhiShiStatus;
    public Vector3 SkillTarget;

    private Ray ray;

    public bool skillInnerIsActive;        //圆圈施法
    public bool skillDirIsActive;          //箭头施法
    public bool skillArea60IsActive;       //60度施法
    public bool skillArea120IsActive;      //120度施法

    private int ttttNumber;

    // Use this for initialization
    void Start()
    {
        skillInnerIsActive = false;
        skillDirIsActive = false;
        skillArea60IsActive = false;
        skillArea120IsActive = false;
        ttttNumber = 0;
        /*
        SkillGameObj = GameObject.Find("SkillGameObj").transform;
        Skill_Area = SkillGameObj.GetChild(0).gameObject;
        Skill_Dir = SkillGameObj.GetChild(1).gameObject;
        Skill_Area_60 = SkillGameObj.GetChild(2).gameObject;
        Skill_Area_120 = SkillGameObj.GetChild(3).gameObject;
        Skill_InnerArea = SkillGameObj.GetChild(4).gameObject;
        */

        //初始隐藏全部
        for (int i = 0; i < SkillGameObj.childCount; i++)
        {
            SkillGameObj.GetChild(i).gameObject.SetActive(false);
        }

        RoseObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
    }

    // Update is called once per frame
    void Update()
    {

        //设置自身始终跟随主角
        SkillGameObj.position = RoseObj.transform.position;

        if (skillInnerIsActive) {

        }

        //箭头施法
        if (skillDirIsActive) {
            UseSkillDir(SkillTarget);
        }

        //60°施法
        if (skillArea60IsActive)
        {         
            UseSkill60(SkillTarget);
        }

        //120°施法
        if (skillArea120IsActive)
        {
            UseSkill120(SkillTarget);
        }

    }

    public void InitSkillTarget(Vector3 target)
    {
        SkillTarget = target;
    }

    //展示箭头
    public void Show_SkillDir() {
        skillDirIsActive = true;
        SkillAreaController(SkillAreaType.Skill_Dir, true);
    }

    //展示60°
    public void Show_SkillArea60r()
    {
        skillArea60IsActive = true;
        SkillAreaController(SkillAreaType.Skill_Angle, true);
    }

    //占时120°
    public void Show_SkillArea120r()
    {
        skillArea120IsActive = true;
        SkillAreaController(SkillAreaType.Skill_Angle, true);
    }


    //清理所有显示
    public void ClearnsShow() {
        ttttNumber = 0;
        skillInnerIsActive = false;
        skillDirIsActive = false;
        skillArea60IsActive = false;
        skillArea120IsActive = false;
        //初始隐藏全部
        for (int i = 0; i < SkillGameObj.childCount; i++)
        {
            SkillGameObj.GetChild(i).gameObject.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 内圆技能
    /// </summary>
    /// <param name="target"></param>
    private void UseSkillInner(Vector3 target)
    {
        if (!Skill_Area.activeInHierarchy)
        {
            SkillAreaController(SkillAreaType.Skill_InnerCirle, true);
        }
        else
        {
            if (Vector3.Distance(target, SkillGameObj.transform.position) <= outerRadius * 2)
            {
                Skill_InnerArea.transform.position = target;
            }
            else
            {
                Skill_InnerArea.transform.localPosition = (target - Skill_Area.transform.position).normalized * outerRadius;
                //Skill_InnerArea.transform.position = (target - Skill_Area.transform.position).normalized * outerRadius * 2;
            }
            Collider[] enemy = Physics.OverlapSphere(Skill_InnerArea.transform.position, innerRadius, 1 << LayerMask.NameToLayer("Enemy"));
            if (enemy.Length > 0)
            {
                Debug.Log(enemy[0].name);
            }
        }
        if (Input.GetMouseButton(0))
        {
            skillInnerIsActive = false;
            SkillAreaController(SkillAreaType.Skill_InnerCirle, skillInnerIsActive);//隐藏技能圈
            transform.LookAt(target);
        }
    }

    /// <summary>
    /// 直线技能
    /// </summary>
    /// <param name="target"></param>
    public void UseSkillDir(Vector3 target)
    {
        Skill_Area.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.transform.position;
        if (!Skill_Area.activeInHierarchy)
        {
            //Skill_Dir.SetActive ( false);
            SkillAreaController(SkillAreaType.Skill_Dir, skillDirIsActive);
        }
        else
        {
            if (skillDirIsActive && !Skill_Dir.activeSelf)
                ttttNumber++;
            if (ttttNumber >= 2 && !Skill_Dir.activeSelf)
                Skill_Dir.SetActive(true);

           Skill_Dir.transform.LookAt(target);
           Skill_Dir.transform.localEulerAngles = new Vector3(1, Skill_Dir.transform.localEulerAngles.y, Skill_Dir.transform.localEulerAngles.z);
           
            //Skill_Dir.transform.localRotation = new Quaternion(1,Skill_Dir.transform.localRotation.y, Skill_Dir.transform.localRotation.z, Skill_Dir.transform.localRotation.w);
            //Debug.Log("222222222222");
            /*
            Collider[] enemy = Physics.OverlapBox(Skill_Dir.transform.GetChild(0).position, Skill_Dir.transform.GetChild(0).localScale, Skill_Dir.transform.rotation, 1 << LayerMask.NameToLayer("Enemy"));
            if (enemy.Length > 0)
            {
                Debug.Log(enemy[0].name);
            }
            */
        }
        /*
        if (Input.GetMouseButton(0))
        {
            skillDirIsActive = false;
            SkillAreaController(SkillAreaType.Skill_Dir, skillDirIsActive);//隐藏技能圈
            transform.LookAt(target);
        }
        */
    }

    /// <summary>
    /// 60度技能
    /// </summary>
    /// <param name="target"></param>
    private void UseSkill60(Vector3 target)
    {

        angle = 60;
        Skill_Area.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.transform.position;
        Skill_Area.SetActive(true);
        if (!Skill_Area_60.activeInHierarchy)
        {
            Skill_Area_60.transform.LookAt(target);
            SkillAreaController(SkillAreaType.Skill_Angle, skillArea60IsActive);
        }
        else
        {
            Skill_Area_60.transform.LookAt(target);
            Skill_Area_60.transform.localEulerAngles = new Vector3(1, Skill_Area_60.transform.localEulerAngles.y, Skill_Area_60.transform.localEulerAngles.z);
            /*
            Collider[] enemy = Physics.OverlapSphere(transform.position, outerRadius * 2, 1 << LayerMask.NameToLayer("Enemy"));
            if (enemy.Length > 0)
            {
                for (int i = 0; i < enemy.Length; i++)
                {
                    float distance = Vector3.Distance(transform.position, enemy[i].transform.position);//距离
                    Vector3 temVec = enemy[i].transform.position - transform.position;
                    float tempAngle = Mathf.Acos(Vector3.Dot(Skill_Area_60.transform.forward.normalized, temVec.normalized)) * Mathf.Rad2Deg;//计算两个向量间的夹角
                    if (tempAngle < angle / 2 && distance <= outerRadius * 2)
                    {
                        Debug.Log(enemy[i].name);
                    }
                }
            }
            */
        }

        if (skillArea60IsActive && !Skill_Area_60.activeSelf)
            ttttNumber++;
        if (ttttNumber >= 2 && !Skill_Area_60.activeSelf)
            Skill_Area_60.SetActive(true);

    }

    /// <summary>
    /// 120度技能
    /// </summary>
    /// <param name="target"></param>
    private void UseSkill120(Vector3 target)
    {
        angle = 120;
        if (!Skill_Area_120.activeInHierarchy)
        {
            Skill_Area.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.transform.position;
            SkillAreaController(SkillAreaType.Skill_Angle, skillArea120IsActive);
        }
        else
        {
            Skill_Area_120.transform.LookAt(target);
            Skill_Area_120.transform.localEulerAngles = new Vector3(1, Skill_Area_120.transform.localEulerAngles.y, Skill_Area_120.transform.localEulerAngles.z);
            /*
            Collider[] enemy = Physics.OverlapSphere(transform.position, outerRadius * 2, 1 << LayerMask.NameToLayer("Enemy"));
            if (enemy.Length > 0)
            {
                for (int i = 0; i < enemy.Length; i++)
                {
                    float distance = Vector3.Distance(transform.position, enemy[i].transform.position);//距离
                    Vector3 temVec = enemy[i].transform.position - transform.position;
                    float tempAngle = Mathf.Acos(Vector3.Dot(Skill_Area_120.transform.forward.normalized, temVec.normalized)) * Mathf.Rad2Deg;//计算两个向量间的夹角
                    if (tempAngle < angle / 2 && distance <= outerRadius * 2)
                    {
                        Debug.Log(enemy[i].name);
                    }
                }
            }
            */
        }

    }

    //显示
    private void SkillAreaController(SkillAreaType areaType, bool skillIsActive)
    {

        Skill_Area.gameObject.SetActive(skillDirIsActive);
        Skill_Area.transform.localScale = new Vector3(outerRadius * 2, outerRadius * 2, outerRadius * 2);

        switch (areaType)
        {
            case SkillAreaType.Skill_InnerCirle:
                Skill_InnerArea.gameObject.SetActive(skillIsActive);
                Skill_InnerArea.transform.localScale = new Vector3(innerRadius * 2, innerRadius * 2, innerRadius * 2);
                break;
            case SkillAreaType.Skill_Dir:

                Skill_Dir.transform.localScale = new Vector3(cubeWidth, Skill_Dir.transform.localScale.y, Skill_Dir.transform.localScale.z);
                Vector3 SelectRangeEffect;
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget)
                {
                    SelectRangeEffect = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.transform.TransformPoint(0, 0, 0);
                }
                else
                {
                    SelectRangeEffect = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.transform.TransformPoint(0, 0, 1);  //此代码表示技能指示为最前方
                }
                Skill_Dir.transform.LookAt(SelectRangeEffect);
                break;
            case SkillAreaType.Skill_Angle:
                switch (this.angle)
                {
                    case 60:
                        if (!skillIsActive)
                            Skill_Area_60.transform.gameObject.SetActive(skillIsActive);
                        break;
                    case 120:
                        Skill_Area_120.transform.gameObject.SetActive(skillIsActive);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }
}
