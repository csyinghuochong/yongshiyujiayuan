using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType
{
    One = 1, Two, Three, Four, Five, Six, None
}

public class TouZi_Point : MonoBehaviour
{
    public PointType selfType;
    public PointType OppositeTypeOld;
    public PointType OppositeType = PointType.None;

    private void Awake()
    {
        //OppositeTypeOld = OppositeType;
        //OppositeType = PointType.None;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.layer == 10) {
            OppositeType = OppositeTypeOld;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        OppositeType = PointType.None;
    }

    public void ResetOpposite()
    {
        OppositeType = PointType.None;
    }


    /// <summary>
    /// 数学计算的方式
    /// </summary>
    /// <returns></returns>
    public float GetDot()
    {
        //Debug.Log(Vector3.Dot(Vector3.up, transform.up)+"    "+ selfType);
        return Vector3.Dot(Vector3.up, transform.up);
    }
}