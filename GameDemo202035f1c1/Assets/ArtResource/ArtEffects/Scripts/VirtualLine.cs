using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Com.Game.Mono
{
    public class VirtualLine : MonoBehaviour
    {
        public Transform scaleBase;
        private Material mMaterial;

        // Use this for initialization
        void Start()
        {
            this.mMaterial = this.GetComponent<Renderer>().material;
        }

        void Update()
        {
            if (this.scaleBase)
            {
                if (this.mMaterial)
                {
                    this.mMaterial.SetFloat("_Scale", this.scaleBase.localScale.z);
                }
                else
                {
                    this.mMaterial = this.GetComponent<Renderer>().material;
                }
            }

        }
    }
}

