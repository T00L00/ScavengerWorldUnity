using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Drop Off")]
    public class DropOff : ActionLogic
    {
        public AnimationClip animation;

        public override bool RequiresInRange(ActionData data)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.HowFullIsInventory > 0f
                && unit.TeamId == target.Unit.TeamId;
        }

        public override void StartAction(ActionData data)
        {
            data.unit.AIController.FaceTowards(data.target);
            data.unit.AIController.AnimateAction(animation);
        }

        public override void StopAction(ActionData data)
        {
            int food = data.unit.RemoveAllItems();
            data.target.Unit.AddItem(food);
            //unit.SetReward(food * 0.01f);
            data.unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(ActionData data)
        {
            
        }
    }
}