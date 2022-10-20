using ScavengerWorld.AI.UAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScavengerWorld.AI
{
    public class CombatAIState : AIState
    {
        public CombatAIState(NavMeshAgent navigator, Unit unit, AnimationController animController)
            : base(navigator, unit, animController)
        {
            this.navigator = navigator;
            this.unit = unit;
            this.animController = animController;
            this.locomotion = unit.Weapon?.combatLocomotion;
            this.ai = new CombatUtilityAI();
            this.state = State.Combat;
            actionProgress = 0;
        }

        public void SetTarget(Interactable target)
        {
            ai.Target = target;
        }
    }
}