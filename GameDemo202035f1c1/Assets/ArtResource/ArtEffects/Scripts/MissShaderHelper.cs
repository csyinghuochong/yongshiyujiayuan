using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Com.Game.Mono
{
    public class MissShaderHelper : MonoBehaviour
    {
        public bool isProjector = false;
        // Use this for initialization

        void ResetMaterial(Material mtl)
        {
            if (mtl && mtl.shader)
            {
                Shader shader = Shader.Find(mtl.shader.name);
                if (shader)
                {
                    mtl.shader = shader;
                }
            }
        }
        void Start()
        {
            if (this.isProjector)
            {
                Projector projector = this.GetComponent<Projector>();
                if (projector)
                {
                    this.ResetMaterial(projector.material);
                }
            }
            else
            {
                if (this.GetComponent<Renderer>())
                {
                    foreach (Material mtl in this.GetComponent<Renderer>().materials)
                    {
                        this.ResetMaterial(mtl);
                    }
                }
            }


        }

    }
}