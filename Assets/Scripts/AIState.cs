using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Animancer;
using Animancer.FSM;
using ScavengerWorld.AI.UAI;
using System.Linq;

namespace ScavengerWorld.AI
{
    public class AIState : IState
    {
        public enum State
        {
            Default,
            Combat
        }

        protected NavMeshAgent navigator;
        protected Unit unit;
        protected AnimationController animController;
        protected LinearMixerTransition locomotion;

        protected State state;
        protected UtilityAI ai;
        protected Action selectedAction;
        protected float actionProgress;

        protected InteractNode targetNode;
        protected Vector3 facing;

        public bool MoveEnabled { get; private set; }

        public float ActionProgress => actionProgress;

        public bool Enabled { get; set; }

        public virtual bool CanEnterState => Enabled;

        public virtual bool CanExitState => !Enabled;

        public AIState(NavMeshAgent navigator, Unit unit, AnimationController animController)
        {
            this.navigator = navigator;
            this.unit = unit;
            this.animController = animController;
            this.locomotion = unit.DefaultLocomotion;
            this.ai = new UtilityAI();
            this.state = State.Default;
            actionProgress = 0;
            MoveEnabled = true;
        }

        public State GetState() => state;

        public virtual void AnimateLocomotion()
        {
            animController.AnimateLocomotion(locomotion);
        }

        public virtual void OnEnterState()
        {            
            AnimateLocomotion();
        }

        public virtual void OnExitState()
        {
            ai.Reset();
        }

        public virtual void OnUpdate()
        {
            locomotion.State.Parameter = navigator.velocity.magnitude;
            HandleRotation();

            if (selectedAction is null)
            {
                ai.GetUseableActions(unit);
                if (ai.useableActions.Count == 0) return;

                selectedAction = ai.DecideBestAction(unit);
                return;
            }

            // Move to action target

            if (targetNode != null)
            {
                if (HasReachedTarget())
                {
                    if (!selectedAction.IsRunning)
                    {
                        DisableMovement();
                        selectedAction.StartAction(unit);
                    }
                    else
                    {
                        selectedAction.UpdateAction(unit);
                    }
                }
                else
                {
                    EnableMovement();
                    MoveToTargetNode();
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
                    StopMoving();
                    CancelSelectedAction();
                }
            }
        }

        #region Running actions

        public void AddActionProgress(float amount)
        {
            actionProgress += amount;
        }

        public void ResetActionProgress()
        {
            actionProgress = 0f;
        }

        public void SetSelectedAction(Action action)
        {
            CancelSelectedAction();
            selectedAction = action;
        }

        public void CancelSelectedAction()
        {
            selectedAction.StopAction(unit);
        }

        public virtual void OnFinishedAction()
        {
            ClearSelectedAction();
            animController.AnimateLocomotion(locomotion);
        }

        protected void ClearSelectedAction()
        {
            selectedAction = null;
            targetNode?.ClearOccupant();
            targetNode = null;
        }

        #endregion

        #region Movement

        public bool HasReachedTargetNode()
        {
            float distance = Vector3.Distance(unit.transform.position, targetNode.transform.position);
            return Mathf.Abs(distance - navigator.stoppingDistance) <= 0.1f;
        }

        public bool HasReachedTarget()
        {
            float distance = Vector3.Distance(unit.transform.position, targetNode.Parent.transform.position);
            return Mathf.Abs(distance - targetNode.Parent.useRange) <= 0.1f;
        }

        public void MoveToTargetNode()
        {
            if (MoveEnabled)
            {
                navigator.SetDestination(targetNode.transform.position);
            }
        }

        public void StopMoving()
        {
            navigator.velocity = Vector3.zero;
            navigator.ResetPath();
        }

        public void EnableMovement()
        {
            MoveEnabled = true;
        }

        public void DisableMovement()
        {
            StopMoving();
            MoveEnabled = false;
        }

        public void FaceTowards(Interactable i)
        {
            facing = i.transform.position - unit.transform.position;
            facing.y = 0f;
            facing.Normalize();

            //Apply Rotation
            Quaternion targ_rot = Quaternion.LookRotation(facing, Vector3.up);
            Quaternion nrot = Quaternion.RotateTowards(unit.transform.rotation, targ_rot, 360f);
            unit.transform.rotation = nrot;
        }

        protected void HandleRotation()
        {
            if (navigator.hasPath)
            {
                facing = navigator.steeringTarget - unit.transform.position;
                facing.y = 0f;
                facing.Normalize();

                //Apply Rotation
                Quaternion targ_rot = Quaternion.LookRotation(facing, Vector3.up);
                Quaternion nrot = Quaternion.Slerp(unit.transform.rotation, targ_rot, unit.RotateSpeed * Time.deltaTime);
                unit.transform.rotation = nrot;
            }
        }

        #endregion
    }
}