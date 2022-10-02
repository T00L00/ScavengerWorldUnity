using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    [System.Serializable]
    public class UtilityAction : Action
    {
        public float weight = 1f;
        public Consideration[] considerations;

        public float Score { get; set; }

        public override bool IsEmpty => actionLogic is null;

        public UtilityAction(ActionLogic actionLogic, float weight, Consideration[] considerations)
        {
            this.actionLogic = actionLogic;
            this.weight = weight;
            this.considerations = considerations;
        }

        public SerializedUtilityAction Serialize()
        {
            return new SerializedUtilityAction(this);
        }
    }
}