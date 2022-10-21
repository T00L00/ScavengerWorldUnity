using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI;

namespace ScavengerWorld
{
    /// <summary>
    /// Action that tells AI to switch to combat state
    /// </summary>
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Attack")]
    public class Attack : ActionLogic
    {
        public override bool RequiresInRange(Unit unit, Interactable target)
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

        public override void StartAction(Unit unit, Interactable target)
        {
            //unit.Attack(target.Damageable);
            //if (target.Damageable.CurrentHealth == 0f)
            //{
            //    unit.SetReward(0.05f);
            //    target.Unit?.SetReward(-0.05f);
            //}
            //StopAction(unit, target);
            //Debug.Log("Selected action: Attack");
            unit.AIController.SetState(AIState.State.Combat, target);
            StopAction(unit, target);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            
        }
    }
}