using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Drop Off")]
    public class DropOff : ActionLogic
    {
        public override bool RequiresInRange(Unit unit, Interactable target)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.HowFullIsInventory > 0f
                && unit.TeamId == target.Unit.TeamId;
        }

        public override void StartAction(Unit unit, Interactable target)
        {
            unit.AIController.FaceTowards(target);
            unit.AnimController.AnimateAction(animation);

            var food = unit.RemoveAllItems();
            target.Unit.AddItem(food);
            unit.SetReward(food * 0.01f);          
        }

        public override void StopAction(Unit unit, Interactable target)
        {            
            unit.AnimController.StopActionAnimation();
            unit.AIController.OnFinishedAction();
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