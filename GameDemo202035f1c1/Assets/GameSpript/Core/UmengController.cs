using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weijing;
using Umeng;


public class UmengController : Singleton <UmengController>

{

	/// <summary>
	/// 基本事件
	/// </summary>
	/// <param name="eventId">友盟后台设定的事件Id</param>
	public void Event(string eventId)
	{
		GA.Event(eventId);
	}

	/// <summary>
	/// 基本事件
	/// </summary>
	/// <param name="eventId">友盟后台设定的事件Id</param>
	/// <param name="label">分类标签</param>

	public void Event(string eventId, string label)
	{
		GA.Event(eventId, label);
	}

	/// <summary>
	/// 属性事件
	/// </summary>
	/// <param name="eventId">友盟后台设定的事件Id</param>
	/// <param name="attributes"> 属性中的Key-Vaule Pair不能超过10个</param>
	public void Event(string eventId, Dictionary<string, string> attributes)
	{
		Debug.Log("unity Event: " + eventId);
		GA.Event(eventId, attributes);
	}


}
