using ScavengerWorld.AI.UAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScavengerWorld.AI
{
    public class CombatAIState : AIState
    {
        public CombatAIState(NavMeshAgent navigator, Unit unit, float rotateSpeed)
            : base(navigator, unit, rotateSpeed)
        {
            this.navigator = navigator;
            this.unit = unit;
            this.rotateSpeed = rotateSpeed;
            this.ai = new CombatUtilityAI();
            this.state = State.Combat;
            actionProgress = 0;
        }

        public void SetTarget(Interactable target)
        {
            ai.Target = target;
        }

        //public override void OnUpdate()
        //{
        //    if (selectedAction is null)
        //    {
        //        ai.GetUseableActions(unit);
        //        if (ai.useableActions.Count == 0) return;

        //        selectedAction = ai.DecideBestAction(unit);
        //        return;
        //    }

        //    HandleRotation();

        //    //TODO - logic to handle movement when in combat state
        //}
    }
}