/* Copyright (c) 2015 3shine.com
 * Anthor：penguin_ku(十月)
 * DateTime：2015/5/3 22:35:41
 * FileName：MainTaskProcessor
 * MachineName: win8.1-04020905
 * Version：V1.0.0.0
 * 
 * Function：
 * 1、
 * 2、
 * 3、
 * 4、
 *
 * Tip:
 * 1、
 * 2、
 * 3、
 * 4、
 *
 * Modify：
 * DateTime：
 * Remark：
*/

//using Assets.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 主线程任务处理器
/// </summary>
public class MainTaskProcessor : MonoBehaviour
{
    #region 私有变量

    private static System.Object m_oneLoopQueueLock = new object();
    private static List<TaskUnit> m_oneLoopQueues = new List<TaskUnit>();

    #endregion

    #region 公开方法

    /// <summary>
    /// 追加一次性任务
    /// </summary>
    /// <param name="p_action"></param>
    public static void AppendOneAction(TaskUnit p_action)
    {
        lock (m_oneLoopQueueLock)
        {
            m_oneLoopQueues.Add(p_action);
        }
    }

    #endregion

    #region 生命周期

    private void Start()
    {
        //gameObject.name = "MainTaskProcessor";
    }

    private void Update()
    {
        //while (true)
        //{

            for (int i = 0; i < m_oneLoopQueues.Count; i++)
            {
                try
                {
                    var item = m_oneLoopQueues[i];
                    item.CurrWait += Time.deltaTime;
                    if (item.CurrWait >= item.Interval)
                    {
                        item.Action();
                        item.CurrWait = 0;
                        item.CurrLoopTimes++;

                        if (item.LoopTimes != -1 && item.CurrLoopTimes >= item.LoopTimes)
                        {
                            m_oneLoopQueues.RemoveAt(i);
                            i--;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("线程报错:" + ex);
                    m_oneLoopQueues.RemoveAt(i);
                    //i--;
                //break;
                }
            }

        //}

    }

    #endregion
}