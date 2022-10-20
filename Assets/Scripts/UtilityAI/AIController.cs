using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;
using UnityEngine.Events;
using Animancer.FSM;
using UnityEngine.UI;
using UnityEngine.AI;

namespace ScavengerWorld.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 20f;

        private Unit unit;
        private AnimationController animController;
        private NavMeshAgent navigator;

        private AIState defaultState;
        private CombatAIState combatState;

        private StateMachine<AIState>.WithDefault stateMachine;

        public AIState.State CurrentState => stateMachine.CurrentState.GetState();

        public float ActionProgress => stateMachine.CurrentState.ActionProgress;

        public float NavigatorSpeed => navigator.velocity.magnitude;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            animController = GetComponent<AnimationController>();
            navigator = GetComponent<NavMeshAgent>();
            navigator.autoRepath = true;
            navigator.updateRotation = false;

            defaultState = new AIState(navigator, unit, animController);
            combatState = new CombatAIState(navigator, unit, animController);

            defaultState.Enabled = true;
            combatState.Enabled = false;
            stateMachine = new StateMachine<AIState>.WithDefault(defaultState);
        }

        // Start is called before the first frame update
        void Start()
        {
            stateMachine.TrySetDefaultState();
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.CurrentState.OnUpdate();
        }

        public void AnimateAction(AnimationClip clip)
        {
            DisableMovement();
            animController.AnimateAction(clip);
        }

        public void StopActionAnimation()
        {
            animController.StopActionAnimation();
            stateMachine.CurrentState.AnimateLocomotion();
            EnableMovement();
        }

        public void AnimateStagger()
        {
            DisableMovement();
            animController.AnimateStagger();
        }

        public void AnimateDeath()
        {
            DisableMovement();
            animController.AnimateDeath(unit.DeathAnimation);
        }

        public void StopMoving()
        {
            stateMachine.CurrentState.StopMoving();
        }

        public void EnableMovement()
        {
            stateMachine.CurrentState.EnableMovement();
        }

        public void DisableMovement()
        {
            stateMachine.CurrentState.DisableMovement();
        }

        public void FaceTowards(Interactable i)
        {
            stateMachine.CurrentState.FaceTowards(i);
        }

        public void SetState(AIState.State state, Interactable target=null)
        {
            switch (state)
            {
                case AIState.State.Default:
                    defaultState.Enabled = true;
                    combatState.Enabled = false;
                    stateMachine.TrySetState(defaultState);
                    break;

                case AIState.State.Combat:

                    defaultState.Enabled = false;
                    combatState.Enabled = true;
                    combatState.SetTarget(target);
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