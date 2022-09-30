using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace ScavengerWorld.AI.UtilityAI
{
    [System.Serializable]
    public class UtilityAction : Action
    {
        public float Weight = 1f;
        public Consideration[] considerations;

        public float Score { get; set; }

        public override bool IsEmpty => actionLogic is null;
    }
}