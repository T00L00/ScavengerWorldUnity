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
        public float energyCost;

        public override bool RequiresInRange(Unit unit, Interactable target)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.AIController.AIState == AIState.Combat
                && unit.Attributes.Energy.CurrentValue >= energyCost;
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

            //Debug.Log($"{unit.gameObject.name} does attack move!");

            unit.Mover.FaceTowards(target);
            unit.AnimController.AnimateAttackAction(animation);
            unit.Attributes.Energy.Reduce(energyCost);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            if (target.Unit.Damageable.IsAlive)
            {
                unit.ActionRunner.ClearCurrentAction();
                return;
            }

            unit.AIController.SetState(AIState.Default);
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            if (unit.AnimController.ActionIsPlaying)
            {
                return;
            }

            StopAction(unit, target);
        }
    }
}
