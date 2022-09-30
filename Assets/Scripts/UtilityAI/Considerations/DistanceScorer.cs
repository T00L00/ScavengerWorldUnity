using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UtilityAI
{
    [CreateAssetMenu(menuName = "Scavenger World/AI/Utility AI/Consideration Scorers/Distance Scorer")]
    public class DistanceScorer : ConsiderationScorer
    {
        public override float ScoreConsideration(Unit unit, Interactable actionTarget, AnimationCurve responseCurve)
        {
            float distFromTarget = Vector3.Distance(unit.transform.position, actionTarget.transform.position);
            return responseCurve.Evaluate(distFromTarget/unit.SensorRange);
        }
    }
}