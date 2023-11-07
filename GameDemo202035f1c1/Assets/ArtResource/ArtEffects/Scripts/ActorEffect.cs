using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum EffectUpdateMode
{
    BOTH,
    POSITION,
    NONE
}

namespace Assets.Scripts.Com.Game.Mono
{
    public class ActorEffect : MonoBehaviour
    {
        public EffectUpdateMode updateMode = EffectUpdateMode.BOTH;
        public float duration = 5;
    }
}

