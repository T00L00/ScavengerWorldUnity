using ScavengerWorld.AI.UAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScavengerWorld.AI
{
    public enum CombatActionType
    {
        Attack,
        Defend
    }

    public class CombatAIState : AIState
    {
        public bool IsBlocking { get; set; }

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

        public override void OnEnterState()
        {
            navigator.speed = 8f;
            AnimateLocomotion();
        }

        public override void OnUpdate()
        {
            // Check for all enemies targeting me
            // Decide which enemy to engage based on health, distance, stats
            // If no enemies targeting me, search for enemy unit to engage

            // Will need to constantly check for an update in enemy targets or
            // listen to some event fired when enemy engages or is nearby

            locomotion.State.Parameter = navigator.velocity.magnitude;
            HandleRotation();

            if (selectedAction is null)
            {


                // Decide which enemy target to focus

                // Select attack move
                selectedAction = ai.DecideBestAction(unit);
                selectedAction.Target = ai.Target;
                return;
            }

            if (HasReachedTarget())
            {
                if (!selectedAction.IsRunning)
                {
                    DisableMovement();
                    selectedAction.StartAction();
                }
            }
            else
            {
                MoveToTarget();
            }
        }

        public override bool HasReachedTarget()
        {
            float distance = Vector3.Distance(unit.transform.position, ai.Target.transform.position);
            return distance <= ai.Target.useRange;
        }

        private void MoveToTarget()
        {
            if (MoveEnabled)
            {
                navigator.SetDestination(ai.Target.transform.position);
            }
        }
    }
}