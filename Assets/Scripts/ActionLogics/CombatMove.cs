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
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Combat Move")]
    public class CombatMove : ActionLogic
    {
        //public float energyCost;
        public CombatActionType combatActionType;

        public override bool RequiresInRange(ActionData data)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.AIController.CurrentState == AIState.State.Combat;
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

            //Debug.Log($"{unit.gameObject.name} does attack move!");

            data.unit.AIController.FaceTowards(data.target);
            if (combatActionType == CombatActionType.Defend)
            {
                data.unit.AIController.CombatState.IsBlocking = true;
            }
            data.unit.AIController.AnimateAttackAction(data.animation);
        }

        public override void StopAction(ActionData data)
        {
            // TODO - Logic to decide whether or not to stay in combat state?

            if (!data.target.Damageable.IsAlive)
            {
                data.unit.AIController.SetState(AIState.State.Default);
            }

            if (combatActionType == CombatActionType.Defend)
            {
                data.unit.AIController.CombatState.IsBlocking = false;
            }
            data.unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(ActionData data)
        {

        }
    }
}
