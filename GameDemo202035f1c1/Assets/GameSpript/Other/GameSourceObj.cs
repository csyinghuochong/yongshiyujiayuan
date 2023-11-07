using UnityEngine;
using System.Collections;

public class GameSourceObj : MonoBehaviour {

    private AudioSource audioSource;
    public float playTime;
    private float playTimeSum; 

	// Use this for initialization
	void Start () {
        audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        //float time = audioSource.time;
        //Debug.Log("Time = " + time);

        if(!audioSource.isPlaying){
            //Debug.Log("播放完毕");
            if (playTime != 999999) {
                Destroy(this.gameObject);
            }
        }
        else
        {
            //Debug.Log("播放中");
            if (playTime > 0) {
                playTimeSum = playTimeSum + Time.deltaTime;
                if (playTimeSum >= playTime) {
                    //Debug.Log("播放完毕2222");
                    Destroy(this.gameObject);
                }
            }
        }
	}
}
