using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Com.Game.Mono
{
     class DeactiveSelf : MonoBehaviour
    {
         public float duration = 1;
         private float mActiveTime = 0;
        void Start()
        {
            this.mActiveTime = Time.time;
        }

         void OnEnable()
        {
            this.mActiveTime = Time.time;
        }

         void Update()
         {
             if (Time.time - this.mActiveTime > this.duration)
             {
                 this.gameObject.SetActive(false);
             }
         }
    }
}

