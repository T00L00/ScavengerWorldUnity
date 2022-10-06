using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;


namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/AI/Utility AI/Consideration Scorers/Energy Scorer")]
    public class EnergyScorer : ConsiderationScorer
    {
        public override float ScoreConsideration(Unit unit, Interactable actionTarget, AnimationCurve responseCurve)
        {
            return responseCurve.Evaluate(unit.Attributes.Energy.Percentage);
        }
    }
}