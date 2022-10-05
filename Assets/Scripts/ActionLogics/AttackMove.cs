using ScavengerWorld.AI;
using ScavengerWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI
{
    /// <summary>
    /// Performs attack move specified by animation clip
    /// </summary>
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Attack Move")]
    public class AttackMove : ActionLogic
    {
        public override bool RequiresInRange(Unit unit, Interactable target)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.AIController.AIState == AIState.Combat;
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

            Debug.Log($"{unit.gameObject.name} does attack move!");
            unit.AnimController.AnimateAction(animation);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            //unit.ActionRunner.ClearCurrentAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {

        }
    }
}
