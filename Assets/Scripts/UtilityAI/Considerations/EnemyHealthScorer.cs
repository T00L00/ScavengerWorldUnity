using ScavengerWorld.AI.UAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    [CreateAssetMenu(menuName = "Scavenger World/AI/Utility AI/Consideration Scorers/Enemy Health Scorer")]
    public class EnemyHealthScorer : ConsiderationScorer
    {
        public override float ScoreConsideration(Unit unit, Interactable actionTarget, AnimationCurve responseCurve)
        {
            return responseCurve.Evaluate(actionTarget.Damageable.HealthPercentage);
        }
    }
}