using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    [CreateAssetMenu(menuName = "Scavenger World/AI/Utility AI/Consideration Scorers/Inventory Capacity Scorer")]
    public class InventoryCapacityScorer : ConsiderationScorer
    {
        public override float ScoreConsideration(Unit unit, Interactable actionTarget, AnimationCurve responseCurve)
        {
            return responseCurve.Evaluate(unit.HowFullIsInventory);
        }
    }
}