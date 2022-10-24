using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScavengerWorld.AI
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Patrol")]
    public class Patrol : ActionLogic
    {
        public override bool RequiresInRange(ActionData data)
        {
            return false;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return true;
        }

        public override void StartAction(ActionData data)
        {
            // Pick random position within territory
            // Go to position
            data.unit.AIController.EnableMovement();

            Vector3 pos = Utils.GetRandomPosition(data.unit, data.unit.StorageDepot.transform.position, 30f);
            data.unit.AIController.MoveToPosition(pos);
            //Debug.Log($"Patrolling: Moving to {pos}");
        }

        public override void StopAction(ActionData data)
        {
            data.unit.AIController.ResetTargetPos();
            data.unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(ActionData data)
        {
            if (data.unit.AIController.HasReachedTargetPos())
            {
                StopAction(data);
            }
        }
    }
}