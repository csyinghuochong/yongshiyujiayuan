using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//技能指示器
public enum SkillAreaType
{
    Skill_InnerCirle = 0,
    Skill_Dir = 1,
    Skill_Angle = 2,
}

public class SkillController : MonoBehaviour
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

    private Ray ray;

    private bool skillInnerIsActive;
    private bool skillDirIsActive;
    private bool skillArea60IsActive;
    private bool skillArea120IsActive;

    // Use this for initialization
    void Start()
    {
        skillInnerIsActive = false;
        skillDirIsActive = false;
        skillArea60IsActive = false;
        skillArea120IsActive = false;

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

        //将技能指示器始终跟随鼠标或者触屏
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        LayerMask mask = 1 << LayerMask.NameToLayer("Terrain");
        Skill_InnerArea.transform.localPosition = Vector3.zero;
        Vector3 target = Vector3.zero;
        if (Physics.Raycast(ray, out hitInfo, 100, mask))
        {
            target = hitInfo.point;
            target.y = transform.position.y;
        }



        #region 内圆范围技能
        if (Input.GetKeyDown(KeyCode.Q) && skillDirIsActive == false && skillArea60IsActive == false && skillArea120IsActive == false)
        {
            skillInnerIsActive = !skillInnerIsActive;
            if (skillDirIsActive == false)
                SkillAreaController(SkillAreaType.Skill_InnerCirle, false);
        }
        if (skillInnerIsActive == true)
        {
            UseSkillInner(target);
        }
        #endregion 

        #region 直线范围技能
        if (Input.GetKeyDown(KeyCode.E) && skillInnerIsActive == false && skillArea60IsActive == false && skillArea120IsActive == false)
        {
            skillDirIsActive = !skillDirIsActive;
            if(skillDirIsActive == false)
                SkillAreaController(SkillAreaType.Skill_Dir, false);
        }
        if (skillDirIsActive == true)
        {
            UseSkillDir(target);
        }
        #endregion

        #region 60度角技能
        if (Input.GetKeyDown(KeyCode.R) && skillInnerIsActive == false && skillDirIsActive == false && skillArea120IsActive == false)
        {
            skillArea60IsActive = !skillArea60IsActive;
            if (skillArea60IsActive == false)
                SkillAreaController(SkillAreaType.Skill_Angle, false);
        }
        if (skillArea60IsActive == true)
        {
            UseSkill60(target);
        }
        #endregion

        #region 120度角技能
        if (Input.GetKeyDown(KeyCode.F) && skillInnerIsActive == false && skillDirIsActive == false && skillArea60IsActive == false)
        {
            skillArea120IsActive = !skillArea120IsActive;
            if (skillArea120IsActive == false)
                SkillAreaController(SkillAreaType.Skill_Angle, false);
        }
        if (skillArea120IsActive == true)
        {
            UseSkill120(target);
        }
        #endregion
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
    private void UseSkillDir(Vector3 target)
    {
        if (!Skill_Area.activeInHierarchy)
        {
            SkillAreaController(SkillAreaType.Skill_Dir, skillDirIsActive);
        }
        else
        {
            Skill_Dir.transform.LookAt(target);
            Collider[] enemy = Physics.OverlapBox(Skill_Dir.transform.GetChild(0).position, Skill_Dir.transform.GetChild(0).localScale, Skill_Dir.transform.rotation, 1 << LayerMask.NameToLayer("Enemy"));
            if (enemy.Length > 0)
            {
                Debug.Log(enemy[0].name);
            }
        }
        if (Input.GetMouseButton(0))
        {
            skillDirIsActive = false;
            SkillAreaController(SkillAreaType.Skill_Dir, skillDirIsActive);//隐藏技能圈
            transform.LookAt(target);
        }
    }

    /// <summary>
    /// 60度技能
    /// </summary>
    /// <param name="target"></param>
    private void UseSkill60(Vector3 target)
    {
        angle = 60;
        if (!Skill_Area_60.activeInHierarchy)
        {
            SkillAreaController(SkillAreaType.Skill_Angle, skillArea60IsActive);
        }
        else
        {
            Skill_Area_60.transform.LookAt(target);
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
        }
        if (Input.GetMouseButton(0))
        {
            skillArea60IsActive = false;
            SkillAreaController(SkillAreaType.Skill_Angle, skillArea60IsActive);//隐藏技能圈
            transform.LookAt(target);
        }
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
            SkillAreaController(SkillAreaType.Skill_Angle, skillArea120IsActive);
        }
        else
        {
            Skill_Area_120.transform.LookAt(target);
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
        }
        if (Input.GetMouseButton(0))
        {
            skillArea120IsActive = false;
            SkillAreaController(SkillAreaType.Skill_Angle, skillArea120IsActive);//隐藏技能圈
            transform.LookAt(target);
        }
    }

    private void SkillAreaController(SkillAreaType areaType, bool skillIsActive)
    {
        Skill_Area.gameObject.SetActive(skillIsActive);
        Skill_Area.transform.localScale = new Vector3(outerRadius * 2, outerRadius * 2, outerRadius * 2);
        switch (areaType)
        {
            case SkillAreaType.Skill_InnerCirle:
                Skill_InnerArea.gameObject.SetActive(skillIsActive);
                Skill_InnerArea.transform.localScale = new Vector3(innerRadius * 2, innerRadius * 2, innerRadius * 2);
                break;
            case SkillAreaType.Skill_Dir:
                Skill_Dir.transform.gameObject.SetActive(skillIsActive);
                Skill_Dir.transform.localScale = new Vector3(cubeWidth, Skill_Dir.transform.localScale.y, Skill_Dir.transform.localScale.z);
                break;
            case SkillAreaType.Skill_Angle:
                switch (this.angle)
                {
                    case 60:
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
