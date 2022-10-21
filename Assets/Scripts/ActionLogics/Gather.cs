using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Gather")]
    public class Gather : ActionLogic
    {
        public override bool RequiresInRange(Unit unit, Interactable target)
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

        public override void StartAction(Unit unit, Interactable target)
        {
            //unit.Mover.FaceTowards(target);
            unit.AIController.FaceTowards(target);
            unit.AIController.AnimateAction(animation);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            unit.AIController.ResetActionProgress();
            unit.AIController.StopActionAnimation();
            unit.AIController.OnFinishedAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            if (!target.isActiveAndEnabled || target.Gatherable.AmountAvailable == 0)
            {
                StopAction(unit, target);
                return;
            }

            unit.AIController.AddActionProgress(TheGame.Instance.GameHourPerRealSecond * unit.Attributes.labouring * Time.deltaTime);
            if (unit.AIController.ActionProgress >= 1f)
            {
                unit.AIController.ResetActionProgress();
                unit.AddItem(target.Gatherable, 1);
            }            
        }
    }
}