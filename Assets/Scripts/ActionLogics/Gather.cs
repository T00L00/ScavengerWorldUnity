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
            return unit.HowFullIsInventory < 1f;
        }

        public override void StartAction(Unit unit, Interactable target)
        {
            //unit.AddItem(target.Gatherable);
            //StopAction(unit, target);
            unit.AnimController.AnimateAction(animation);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            unit.AnimController.StopActionAnimation();
            unit.ActionRunner.ClearCurrentAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            unit.ActionRunner.AddActionProgress(TheGame.Instance.GameHourPerRealSecond * unit.Stats.gatherRate * Time.deltaTime);
            if (unit.ActionRunner.ActionProgress >= 1f)
            {
                unit.ActionRunner.ResetActionProgress();
                unit.AddItem(target.Gatherable, 1);
            }

            if (target.Gatherable.AmountAvailable == 0)
            {
                StopAction(unit, target);
            }
        }
    }
}