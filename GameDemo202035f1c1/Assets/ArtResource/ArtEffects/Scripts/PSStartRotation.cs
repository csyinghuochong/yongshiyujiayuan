using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Com.Game.Mono
{
    [ExecuteInEditMode]
    public class PSStartRotation : MonoBehaviour
    {
        public Transform rotationTrans;
        void LateUpdate()
        {

            if (this.GetComponent<ParticleSystem>() && this.rotationTrans)
            {
                this.GetComponent<ParticleSystem>().startRotation = -Mathf.PI / 2 + this.rotationTrans.localEulerAngles.y * Mathf.PI / 180.0f;
            }
        }

        public void OnActorEffectReset()
        {
            this.LateUpdate();
        }
    }
}