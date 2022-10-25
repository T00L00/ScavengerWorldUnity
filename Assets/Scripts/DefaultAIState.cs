using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Animancer;
using Animancer.FSM;
using ScavengerWorld.AI.UAI;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.TextCore.Text;

namespace ScavengerWorld.AI
{
    public class DefaultAIState : AIState
    {
        private UtilityAI ai;
        private InteractNode targetNode;

        public bool Enabled { get; set; }

        public override bool CanEnterState => Enabled;

        public override bool CanExitState => !Enabled;

        public DefaultAIState(Unit unit, NavMeshAgent navigator, AnimationController animController)
        {
            this.unit = unit;
            this.mover = new(unit, navigator);
            this.animController = animController;

            this.locomotion = unit.DefaultLocomotion;
            this.ai = new UtilityAI();
            this.mode = AIMode.Default;
            actionProgress = 0;
        }

        public AIMode GetState() => mode;

        public override void OnEnterState()
        {
            //Debug.Log($"{locomotion.Animations[0].name} | {locomotion.Animations[1].name} | {locomotion.Animations[2].name}");
            mover.SetMaxSpeed(3f);
            AnimateLocomotion();
        }

        public override void OnExitState()
        {
            CancelSelectedAction();
            ai.Reset();
        }

        /// <summary>
        /// Evaluate if there needs to be a change in current activity
        /// </summary>
        public override void Assess()
        {
            ai.GetInteractableActions(unit);
            nextAction = ai.DecideBestAction(unit);

            if (selectedAction is null || !selectedAction.Equals(nextAction))
            {
                CancelSelectedAction();
                selectedAction = nextAction.Copy();
                return;
            }

            if (selectedAction.Equals(nextAction))
            {
                return;
            }

            if (nextAction is null)
            {
                selectedAction = null;
            }
        }

        public override void OnUpdate()
        {
            locomotion.State.Parameter = mover.Speed;
            mover.HandleRotation();

            if (selectedAction is null)
            {
                //ai.GetInteractableActions(unit);
                //selectedAction = ai.DecideBestAction(unit);
                return;
            }

            if (selectedAction.RequiresInRange())
            {
                if (targetNode != null)
                {
                    if (mover.HasReachedTarget(selectedAction.Target))
                    {
                        if (!selectedAction.IsRunning)
                        {
                            mover.DisableMovement();
                            selectedAction.StartAction();
                        }
                        else
                        {
                            selectedAction.UpdateAction();
                        }
                    }
                    else
                    {
                        mover.EnableMovement();
                        mover.MoveToTargetNode(targetNode);
                    }
                }
                else
                {
                    if (selectedAction.Target.InteractionAvailable)
                    {
                        targetNode = selectedAction.Target.GetAvailableInteractNode(this.unit);
                        targetNode.SetOccupant(this.unit);
                    }
                    else
                    {
                        mover.DisableMovement();
                        CancelSelectedAction();
                    }
                }
            }
            else
            {
                if (!selectedAction.IsRunning)
                {
                    mover.DisableMovement();
                    selectedAction.StartAction();
                }
                else
                {
                    selectedAction.UpdateAction();
                }                
            }
        }

        protected override void ClearSelectedAction()
        {
            selectedAction = null;
            targetNode?.ClearOccupant();
            targetNode = null;
        }
    }
}