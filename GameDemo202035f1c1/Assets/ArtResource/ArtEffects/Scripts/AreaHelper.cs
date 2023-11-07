using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Com.Game.Mono
{
    class AreaHelper : MonoBehaviour
    {
        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            int childCount = this.transform.childCount;
            if (childCount > 1)
            {
                for (int i = 0; i < childCount - 1; i++)
                {
                    Gizmos.DrawLine(this.transform.GetChild(i).position, this.transform.GetChild(i + 1).position);
                }
                Gizmos.DrawLine(this.transform.GetChild(childCount - 1).position, this.transform.GetChild(0).position);
            }

        }
    }
}
