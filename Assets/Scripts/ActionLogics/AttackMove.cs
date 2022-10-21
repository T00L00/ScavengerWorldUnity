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
        //public float energyCost;
        public CombatActionType moveType;

        public override bool RequiresInRange(Unit unit, Interactable target)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.AIController.CurrentState == AIState.State.Combat;
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

            unit.AIController.FaceTowards(target);
            if (moveType == CombatActionType.Defend)
            {
                unit.AIController.CombatState.IsBlocking = true;
            }
            unit.AIController.AnimateAttackAction(animation);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            // TODO - Logic to decide whether or not to stay in combat state?

            if (!target.Damageable.IsAlive)
            {
                unit.AIController.SetState(AIState.State.Default);
            }

            if (moveType == CombatActionType.Defend)
            {
                unit.AIController.CombatState.IsBlocking = false;
            }
            unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {

        }
    }
}
