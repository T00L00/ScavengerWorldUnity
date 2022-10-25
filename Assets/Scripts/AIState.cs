using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer.FSM;
using Animancer;
using ScavengerWorld.AI.UAI;
using UnityEngine.AI;

namespace ScavengerWorld.AI
{
    public enum AIMode
    {
        Default,
        Combat
    }

    public abstract class AIState : IState
    {
        protected Unit unit;
        protected Mover mover;
        protected AnimationController animController;
        protected LinearMixerTransition locomotion;

        protected AIMode mode;
        protected Action selectedAction;
        protected Action nextAction;
        protected float actionProgress;

        public AIMode AIMode => mode;
        public Mover Mover => mover;
        public float ActionProgress => actionProgress;

        public abstract bool CanEnterState { get; }

        public abstract bool CanExitState { get; }

        public abstract void OnEnterState();

        public abstract void OnExitState();

        public abstract void OnUpdate();

        public abstract void Assess();

        public virtual void AnimateLocomotion()
        {
            animController.AnimateLocomotion(locomotion);
        }

        #region Executing actions

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
            selectedAction?.StopAction();
        }

        public virtual void OnFinishedAction()
        {
            mover.EnableMovement();
            ClearSelectedAction();
            animController.AnimateLocomotion(locomotion);
        }

        protected virtual void ClearSelectedAction()
        {
            selectedAction = null;
        }

        #endregion
    }
}