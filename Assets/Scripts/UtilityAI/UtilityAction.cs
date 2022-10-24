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

        public UtilityAction(ActionLogic actionLogic, ActionData data) : base(actionLogic, data)
        {
        }

        public override bool IsEmpty => actionLogic is null;

        //public UtilityAction(ActionLogic actionLogic, float weight, Consideration[] considerations)
        //{
        //    this.actionLogic = actionLogic;
        //    this.weight = weight;
        //    this.considerations = considerations;
        //}

        //public UtilityAction(ActionLogic actionLogic, Interactable target)
        //{
        //    this.actionLogic = actionLogic;
        //    this.Target = target;
        //}

        //public UtilityAction Copy()
        //{
        //    return new UtilityAction(actionLogic, Target);
        //}

        public SerializedUtilityAction Serialize()
        {
            return new SerializedUtilityAction(this);
        }
    }
}