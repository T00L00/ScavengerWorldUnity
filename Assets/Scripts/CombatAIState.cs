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
        private CombatUtilityAI ai;

        public bool IsBlocking { get; set; }

        public bool Enabled { get; set; }

        public override bool CanEnterState => Enabled;

        public override bool CanExitState => !Enabled;

        public CombatAIState(Unit unit, NavMeshAgent navigator, AnimationController animController)
        {
            this.unit = unit;
            this.mover = new(unit, navigator);
            this.animController = animController;

            this.locomotion = unit.Weapon?.combatLocomotion;
            this.ai = new CombatUtilityAI();
            this.mode = AIMode.Combat;
            actionProgress = 0;
        }        

        public override void OnEnterState()
        {
            mover.SetMaxSpeed(8f);
            AnimateLocomotion();
            ai.SelectTarget(unit);
        }

        public override void OnExitState()
        {
            ai.Reset();
        }

        public void AddTarget(Interactable target)
        {
            ai.AddTarget(target);
        }

        public override void Assess()
        {
            
        }

        public override void OnUpdate()
        {
            locomotion.State.Parameter = mover.Speed;
            mover.HandleRotation();

            if (selectedAction is null)
            {
                selectedAction = ai.DecideBestAction(unit);
                selectedAction.Target = ai.Target;
                return;
            }

            if (mover.HasReachedTarget(selectedAction.Target))
            {
                if (!selectedAction.IsRunning)
                {
                    mover.DisableMovement();
                    selectedAction.StartAction();
                }
            }
            else
            {
                mover.MoveToTarget(selectedAction.Target);
            }
        }
    }
}