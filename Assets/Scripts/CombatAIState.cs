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

        public override void OnUpdate()
        {
            locomotion.State.Parameter = navigator.velocity.magnitude;
            HandleRotation();

            if (selectedAction is null)
            {
                // Select attack move
                selectedAction = SelectCombatMove();
                selectedAction.Target = ai.Target;
                return;
            }

            if (HasReachedTarget())
            {
                if (!selectedAction.IsRunning)
                {
                    DisableMovement();
                    selectedAction.StartAction(unit);
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
            //Debug.Log(distance);
            return distance <= ai.Target.useRange;
        }

        private void MoveToTarget()
        {
            if (MoveEnabled)
            {
                navigator.SetDestination(ai.Target.transform.position);
            }
        }

        public Action SelectCombatMove()
        {
            System.Random rnd = new();
            string[] choices = { CombatActionType.Attack.ToString(), CombatActionType.Defend.ToString() };
            float[] weights = { unit.Attributes.attack, unit.Attributes.defense };
            string choice = Utils.Choice(rnd, choices, weights);

            if (choice.Equals(CombatActionType.Attack.ToString()))
            {
                return unit.Weapon.RandomAttackAction();
            }
            else if (choice.Equals(CombatActionType.Defend.ToString()))
            {
                return unit.Weapon.RandomDefenseAction();
            }

            return null;
        }
    }
}