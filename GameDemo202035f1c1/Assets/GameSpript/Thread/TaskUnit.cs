/* Copyright (c) 2015 3shine.com
 * Anthor：penguin_ku(十月)
 * DateTime：2015/8/11 10:04:24
 * FileName：ActionUnit
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 执行单元
/// </summary>
public class TaskUnit
{
    #region 公开属性

    /// <summary>
    /// 设置或获取需要执行的操作
    /// </summary>
    public Action Action { set; get; }

    /// <summary>
    /// 设置或获取等待间隔
    /// </summary>
    public float Interval { set; get; }

    /// <summary>
    /// 设置或获取需要循环执行次数
    /// </summary>
    public int LoopTimes { set; get; }

    /// <summary>
    /// 设置或获取当前等待时间
    /// </summary>
    public float CurrWait { set; get; }

    /// <summary>
    /// 设置或获取当前循环次数
    /// </summary>
    public int CurrLoopTimes { set; get; }

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="p_action"></param>
    /// <param name="p_interval"></param>
    /// <param name="p_loopTimes"></param>
    public TaskUnit(Action p_action, float p_interval = 0, int p_loopTimes = 1)
    {
        Action = p_action;
        Interval = p_interval;
        LoopTimes = p_loopTimes;
        CurrWait = 0;
        CurrLoopTimes = 0;
    }
    #endregion
}