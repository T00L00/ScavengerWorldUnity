using ScavengerWorld.AI.UAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [Serializable]
    public class SerializedUtilityAction
    {
        public string actionLogic;
        public float weight;
        public SerializedConsideration[] considerations;

        public SerializedUtilityAction(UtilityAction a)
        {
            actionLogic = a.Name;
            weight = a.weight;
            considerations = new SerializedConsideration[a.considerations.Length];
            for (int i = 0; i < a.considerations.Length; i++)
            {
                considerations[i] = new SerializedConsideration(a.considerations[i]);
            }
        }

        public bool IsEmpty => actionLogic == default && weight == default && considerations == default;

        //public UtilityAction ToUtilityAction()
        //{
        //    Consideration[] considerations = new Consideration[this.considerations.Length];
        //    for (int i = 0; i < this.considerations.Length; i++)
        //    {
        //        considerations[i] = this.considerations[i].ToConsideration();
        //    }
        //    return new UtilityAction(ActionLogic.Load(actionLogic), weight, considerations);
        //}
    }
}