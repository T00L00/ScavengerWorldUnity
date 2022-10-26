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
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Attack")]
    public class Attack : ActionLogic
    {
        public override bool RequiresInRange(ActionData data)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.AIController.AIMode == AIMode.Combat;
        }

        public override void StartAction(ActionData data)
        {
            data.unit.AIController.FaceTowards(data.target);
            data.unit.AIController.AnimateCombatAction(data.animation);
        }

        public override void StopAction(ActionData data)
        {
            // TODO - Logic to decide whether or not to stay in combat state?

            if (!data.target.Damageable.IsAlive)
            {
                data.unit.AIController.SetState(AIMode.Default);
            }
            data.unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(ActionData data)
        {

        }
    }
}
