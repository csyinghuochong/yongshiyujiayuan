using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.Detectors;

public class GameWaGuaJianCe : MonoBehaviour {

    private bool _cheated;

    public float sumTime;
    public float fixTime;

	// Use this for initialization
	void Start () {
        //SpeedHackDetector.StartDetection(JianCeAddSpeed);
        //InjectionDetector.StartDetection(JianCeDll);
	}
	
	// Update is called once per frame
	void Update () {

        sumTime = sumTime + Time.deltaTime;

	}

    void FixedUpdate() {
        fixTime = fixTime + Time.deltaTime;
    }

    //外挂加速检测
    public void JianCeAddSpeed() {
        _cheated = true;
        Debug.Log("小兔崽子！学什么不好学开加速！！！");
        Debug.Log("小兔崽子！学什么不好学开加速！！！");
        Debug.Log("小兔崽子！学什么不好学开加速！！！");
        Game_PublicClassVar.Get_function_UI.GameHint("小兔崽子！学什么不好学开加速！！！");
        //Application.Quit();
    }

    public void JianCeDll() {
        Debug.Log("小兔崽子！学什么不好学开DLL！！！");
        Debug.Log("小兔崽子！学什么不好学开DLL！！！");
        Debug.Log("小兔崽子！学什么不好学开DLL！！！");
        Debug.Log("小兔崽子！学什么不好学开DLL！！！");
        Game_PublicClassVar.Get_function_UI.GameHint("小兔崽子！学什么不好学开DLL！！！");
    }
}
