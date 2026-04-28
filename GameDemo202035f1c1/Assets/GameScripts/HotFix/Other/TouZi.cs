
using System;
using System.Collections;
using System.Collections.Generic;
//using HLCFrame_TouZi;
using UnityEngine;


public class TouZi : MonoBehaviour
{
    /// <summary>
    /// 当骰子静止时，调用的回调。
    /// </summary>
    //public event EventHandler<PointType> PointTypeCallback;

    [Range(10, 20)]
    public float explosionForce = 1;
    [Range(1, 10)]
    public float torque = 1;

    private bool waiting = false;
    private Rigidbody rig;
    [SerializeField]
    public PointType currentType = PointType.None;
    [SerializeField]
    private List<TouZi_Point> touZi_Points = new List<TouZi_Point>();

    public bool TouZhiStatus;
    public bool ttt;
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (TouZhiStatus) {
            TouZhiStatus = false;
            Throw();
        }

        
        if (ttt) {
            ttt = false;
            currentType = GetCurrentType();
        }
        
        //Debug.Log("rig.velocity.sqrMagnitude = " + rig.velocity.sqrMagnitude);
        if (rig.velocity.sqrMagnitude > 0.001)
        {

        }
        else {
            waiting = false;
            currentType = GetCurrentType();
        }

    }

    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    IEnumerator WaitSelfStop()
    {
        yield return new WaitForSeconds(1);
        while (rig.velocity.sqrMagnitude > 0.001)
        {
            yield return null;
        }
        //PointTypeCallback?.Invoke(this, GetCurrentType());
        StopAllCoroutines();
        waiting = false;
    }

    public void Throw()
    {
        if (waiting)
        {
            return;
        }
        waiting = true;

        torque = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(5, 10);
        explosionForce = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(10, 15);

        for (int i = 0; i < touZi_Points.Count; i++)
        {
            touZi_Points[i].ResetOpposite();
        }
        currentType = PointType.None;
        rig.AddExplosionForce(explosionForce, transform.position - new Vector3(0, 0.5f, 0), 1, 0, ForceMode.Impulse);
        rig.AddTorque(UnityEngine.Random.onUnitSphere * torque, ForceMode.Impulse);
        StartCoroutine("WaitSelfStop");
    }

    
    public PointType GetCurrentType()
    {
        for (int i = 0; i < touZi_Points.Count; i++)
        {
            if (touZi_Points[i].OppositeType != PointType.None)
            {
                currentType = touZi_Points[i].OppositeType;
                break;
            }
        }
        return currentType;
    }
    
    
    /*

    ///以下是 数学计算的方式,正确率 得到了提升 而且不消耗性能
    
    float dot = 0, dot1 = 0;
    private PointType GetCurrentType()
    {
        dot = touZi_Points[0].GetDot();
        currentType = touZi_Points[0].selfType;
        for (int i = 0; i < touZi_Points.Count - 1; i++)
        {
            dot1 = touZi_Points[i + 1].GetDot();
            if (dot < dot1)
            {
                dot = dot1;
               currentType = touZi_Points[i + 1].selfType;
            }
        }
        return currentType;
     }
     */
    




}