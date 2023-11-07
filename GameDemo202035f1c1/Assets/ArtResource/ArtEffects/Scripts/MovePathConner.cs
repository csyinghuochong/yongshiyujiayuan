using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Com.Game.Mono
{
    class MovePathConner : AreaHelper
    {
        public enum RouteEnum
        {
            Up = 0,
            Down,
            Middle,
        }
        public RouteEnum route;
        public override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            int childCount = this.transform.childCount;
            if (childCount > 1)
            {
                for (int i = 0; i < childCount - 1; i++)
                {
                    Gizmos.DrawLine(this.transform.GetChild(i).position, this.transform.GetChild(i + 1).position);
                }
            }

        }
    }
}

