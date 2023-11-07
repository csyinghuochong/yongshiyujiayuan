using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Com.Game.Mono
{
     class GrassAreaPoint : MonoBehaviour
    {
        public void OnDrawGizmosSelected()
        {
            if (this.transform.parent != null)
            {
                AreaHelper owner = this.transform.GetComponentInParent<AreaHelper>();
                if (owner)
                {
                    owner.OnDrawGizmosSelected();
                }
            }
        }
    }
}

