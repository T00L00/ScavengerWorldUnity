using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Gather")]
    public class Gather : ActionLogic
    {
        public AnimationClip animation;

        public override bool RequiresInRange(ActionData data)
        {
            return true;
        }

        public override bool IsAchievable(Unit unit, Interactable target)
        {
            return unit.UnitClass == UnitClass.Gatherer
                && unit.HowFullIsInventory < 1f
                && target.Gatherable.AmountAvailable > 0
                && target.InteractNodeAvailable();
        }

        public override void StartAction(ActionData data)
        {
            //unit.Mover.FaceTowards(target);
            data.unit.AIController.FaceTowards(data.target);
            data.unit.AIController.AnimateAction(animation);
        }

        public override void StopAction(ActionData data)
        {
            data.unit.AIController.ResetActionProgress();
            data.unit.AIController.StopActionAnimation();
            data.unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(ActionData data)
        {
            if (!data.target.isActiveAndEnabled || data.target.Gatherable.AmountAvailable == 0)
            {
                StopAction(data);
                return;
            }

            data.unit.AIController.AddActionProgress(TheGame.Instance.GameHourPerRealSecond * data.unit.Attributes.labouring * Time.deltaTime);
            if (data.unit.AIController.ActionProgress >= 1f)
            {
                data.unit.AIController.ResetActionProgress();
                data.unit.AddItem(data.target.Gatherable, 1);
            }            
        }
    }
}