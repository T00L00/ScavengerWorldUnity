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
            navigator = GetComponent<NavMeshAgent>();
            navigator.autoRepath = true;
            navigator.updateRotation = false;

            defaultState = new AIState(navigator, unit, rotateSpeed);
            combatState = new CombatAIState(navigator, unit, rotateSpeed);

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

                    // TODO - Need to somehow pass target into combat state
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
        }
    }
}