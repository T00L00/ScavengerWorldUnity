using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UtilityAI
{
    [Serializable]
    public struct SerializedConsideration
    {
        public string name;
        public float weight;
        public SerializableCurve curve;
        public string considerationScorer;

        public SerializedConsideration(Consideration c)
        {
            name = c.Name;
            weight = c.Weight;
            curve = new SerializableCurve(c.responseCurve);
            considerationScorer = c.scorer.name;
        }

        public Consideration ToConsideration()
        {
            return new Consideration(name, weight, curve.toCurve(), ConsiderationScorer.Load(considerationScorer));
        }
    }
}