using UnityEngine;
using System.Collections;
using System.Data;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;

public class RobotManager : MonoBehaviour
{

	string ipAddress = "";
	int port = 15006;

	public int mMinID = 0;
	public int mMaxID = 0;
	public float mCreateTime = 1f;
	private List<RobotTest> mList = new List<RobotTest>();

	void Start () {

	}

	public void OnClickRobots()
	{
		StartCoroutine(CreateRobots());
	}

	IEnumerator CreateRobots()
	{
		for (int i = mMinID; i < mMaxID; i++)
		{
			CreateRobot(i);
			yield return new WaitForSeconds(mCreateTime);
		}
	}

	public void CreateRobot(int id)
	{
		GameObject go = new GameObject(id.ToString());
		var robot = go.AddComponent<RobotTest>();
		//robot.mID = id;
		//robot.mServerIP = mServerIP;
		//robot.mServerPort = mServerPort;
		//robot.Init();
		//robot.mParent = transform;

		mList.Add(robot);
		gameObject.name = "RobotManager_机器人数量:" + mList.Count;
	}


	void Update () {
	
	}

	void OnDisable()
	{
		for (int i = 0; i < mList.Count; i++)
		{
			RobotTest robot = mList[i];
			robot.CloseClient();
		}
	}


	private void ConnectServer()
	{
		

	}
   

}
