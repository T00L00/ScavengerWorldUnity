using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI;

namespace ScavengerWorld
{
    /// <summary>
    /// Action that tells AI to switch to combat state
    /// </summary>
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/EngageCombat")]
    public class EngageCombat : ActionLogic
    {
        public override bool RequiresInRange(ActionData data)
        {
            return false;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.UnitClass == UnitClass.Warrior 
                && unit.Interactable != target
                && target.Damageable.IsAlive
                && unit.TeamId != target.Unit.TeamId;
        }

        public override void StartAction(ActionData data)
        {
            //unit.Attack(target.Damageable);
            //if (target.Damageable.CurrentHealth == 0f)
            //{
            //    unit.SetReward(0.05f);
            //    target.Unit?.SetReward(-0.05f);
            //}
            //StopAction(unit, target);
            //Debug.Log("Selected action: Attack");
            data.unit.AIController.SetState(AIMode.Combat, data.target);
            StopAction(data);
        }

        public override void StopAction(ActionData data)
        {
            data.unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(ActionData data)
        {
            
        }
    }
}