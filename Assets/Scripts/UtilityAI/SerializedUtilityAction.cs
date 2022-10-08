using ScavengerWorld.AI.UAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [Serializable]
    public struct SerializedUtilityAction
    {
        public ActionLogic actionLogic;
        public float weight;
        public Consideration[] considerations;

        private Interactable target;

        public void SetTarget(Interactable target)
        {
            this.target = target;
        }

        public UtilityAction ToUtilityAction()
        {
            return new UtilityAction(actionLogic, target, considerations, weight);
        }
    }
}