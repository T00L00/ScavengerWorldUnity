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

        public override bool IsEmpty => actionLogic is null;

        public UtilityAction(ActionLogic actionLogic,
                             Interactable target,
                             Consideration[] considerations=null,
                             float weight=1f)
        {
            this.actionLogic = actionLogic;
            this.Target = target;

            this.weight = weight;
            this.considerations = considerations;
            this.IsRunning = false;
        }

        public UtilityAction Copy()
        {
            return new UtilityAction(actionLogic, Target);
        }
    }
}