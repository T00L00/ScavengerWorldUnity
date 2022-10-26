using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;
using UnityEngine.Events;
using Animancer.FSM;
using UnityEngine.UI;
using UnityEngine.AI;
using JetBrains.Annotations;

namespace ScavengerWorld.AI
{
    public class AIController : MonoBehaviour
    {
        private Unit unit;
        private AnimationController animController;
        private NavMeshAgent navigator;

        private DefaultAIState defaultState;
        private CombatAIState combatState;

        private StateMachine<AIState>.WithDefault stateMachine;

        public AIMode AIMode => stateMachine.CurrentState.AIMode;

        public float ActionProgress => stateMachine.CurrentState.ActionProgress;
        public DefaultAIState DefaultState => defaultState;
        public CombatAIState CombatState => combatState;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            animController = GetComponent<AnimationController>();
            navigator = GetComponent<NavMeshAgent>();
            navigator.autoRepath = true;
            navigator.updateRotation = false;

            defaultState = new DefaultAIState(unit, navigator, animController);
            combatState = new CombatAIState(unit, navigator, animController);

            defaultState.Enabled = true;
            combatState.Enabled = false;
            stateMachine = new StateMachine<AIState>.WithDefault(defaultState);
        }

        // Start is called before the first frame update
        void Start()
        {
            stateMachine.TrySetDefaultState();
            InvokeRepeating("Assess", 0.5f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.CurrentState.OnUpdate();
        }

        public void Disable()
        {
            navigator.enabled = false;
            enabled = false;
        }

        private void Assess()
        {
            stateMachine.CurrentState.Assess();
        }

        public void AnimateAction(AnimationClip clip)
        {
            animController.AnimateAction(clip);
        }

        public void AnimateCombatAction(AnimationClip clip)
        {
            animController.AnimateAction(clip, unit.Attributes.AttackSpeed);
        }

        public void StopActionAnimation()
        {
            animController.StopActionAnimation();
            stateMachine.CurrentState.AnimateLocomotion();
            EnableMovement();
        }

        public void AnimateStagger()
        {
            animController.AnimateStagger();
        }

        public void AnimateDeath()
        {
            animController.AnimateDeath(unit.DeathAnimation);
        }

        public void EnableMovement()
        {
            stateMachine.CurrentState.Mover.EnableMovement();
        }

        public void DisableMovement()
        {
            stateMachine.CurrentState.Mover.DisableMovement();
        }

        public bool HasReachedTargetPos()
        {
            return stateMachine.CurrentState.Mover.HasReachedTargetPos();
        }

        public void ResetTargetPos()
        {
            stateMachine.CurrentState.Mover.ResetTargetPos();
        }

        public void MoveToPosition(Vector3 pos)
        {
            stateMachine.CurrentState.Mover.MoveToPosition(pos);
        }

        public void FaceTowards(Interactable i)
        {
            stateMachine.CurrentState.Mover.FaceTowards(i);
        }

        public void SetState(AIMode mode, Interactable target=null)
        {
            switch (mode)
            {
                case AIMode.Default:
                    defaultState.Enabled = true;
                    combatState.Enabled = false;
                    stateMachine.TrySetState(defaultState);
                    break;

                case AIMode.Combat:
                    defaultState.Enabled = false;
                    combatState.Enabled = true;
                    combatState.AddTarget(target);
                    stateMachine.TrySetState(combatState); 
                    break;

                default:
                    break;
            }
        }

        public void AddActionProgress(float amount)
        {
            stateMachine.CurrentState.AddActionProgress(amount);
        }

        public void ResetActionProgress()
        {
            stateMachine.CurrentState.ResetActionProgress();
        }

        public void SetSelectedAction(Action action)
        {
            stateMachine.CurrentState.SetSelectedAction(action);
        }

        public void CancelSelectedAction()
        {
            stateMachine.CurrentState.CancelSelectedAction();
        }

        public void OnFinishedAction()
        {
            stateMachine.CurrentState.OnFinishedAction();
            EnableMovement();
        }
    }
}