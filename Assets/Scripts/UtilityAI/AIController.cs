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
    public enum AIState
    {
        Default,
        Combat
    }

    public class AIController : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 20f;

        private Unit unit;
        private NavMeshAgent navigator;

        private CombatUtilityAI combatAI;
        private UtilityAI defaultAI;
        private UtilityAI currentAI;
        private AIState aiState;
        private Action selectedAction;
        private float actionProgress;

        private InteractNode targetNode;
        private Vector3 facing;

        public Interactable Target => selectedAction.Target;
        public bool MoveEnabled { get; private set; }
        public float NavigatorSpeed => navigator.velocity.magnitude;
        public AIState AIState => aiState;
        public Action SelectedAction => selectedAction;
        public float ActionProgress => actionProgress;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            navigator = GetComponent<NavMeshAgent>();
            navigator.autoRepath = true;
            navigator.updateRotation = false;
            MoveEnabled = true;

            aiState = AIState.Default;
            combatAI = new CombatUtilityAI();
            defaultAI = new UtilityAI();
            currentAI = defaultAI;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            // Decide Action

            if (selectedAction is null)
            {
                currentAI.GetUseableActions(unit);
                if (currentAI.useableActions.Count == 0) return;

                selectedAction = currentAI.DecideBestAction(unit);
                return;
            }

            HandleRotation();

            // Move to action target

            if (targetNode != null)
            {
                if (HasReachedTargetNode())
                {
                    if (!selectedAction.IsRunning)
                    {
                        StopMoving();
                        selectedAction.StartAction(unit);
                    }
                    else
                    {
                        selectedAction.UpdateAction(unit);
                    }
                }
                else
                {
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
                    CancelSelectedAction();
                }
            }            
        }

        public bool HasReachedTargetNode()
        {
            float distance = Vector3.Distance(transform.position, targetNode.transform.position);
            return Mathf.Abs(distance - navigator.stoppingDistance) <= 0.01;
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

        public void HandleRotation()
        {
            if (navigator.hasPath)
            {
                facing = navigator.steeringTarget - transform.position;
                facing.y = 0f;
                facing.Normalize();

                //Apply Rotation
                Quaternion targ_rot = Quaternion.LookRotation(facing, Vector3.up);
                Quaternion nrot = Quaternion.Slerp(transform.rotation, targ_rot, rotateSpeed * Time.deltaTime);
                transform.rotation = nrot;
            }
        }

        public void FaceTowards(Interactable i)
        {
            facing = i.transform.position - transform.position;
            facing.y = 0f;
            facing.Normalize();

            //Apply Rotation
            Quaternion targ_rot = Quaternion.LookRotation(facing, Vector3.up);
            Quaternion nrot = Quaternion.RotateTowards(transform.rotation, targ_rot, 360f);
            transform.rotation = nrot;
        }

        public void SetState(AIState state, Interactable target=null)
        {
            switch (state)
            {
                case AIState.Default:
                    selectedAction = null;
                    combatAI.Reset();

                    aiState = state;
                    currentAI = defaultAI;
                    break;
                case AIState.Combat:
                    defaultAI.Reset();
                    aiState = state;
                    combatAI.Target = target;
                    currentAI = combatAI;
                    break;
                default:
                    break;
            }
        }

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
            ClearSelectedAction();
        }

        public void OnFinishedAction()
        {
            selectedAction.IsRunning = false;
            selectedAction = null;
            ClearSelectedAction();
        }

        private void ClearSelectedAction()
        {
            selectedAction = null;
            targetNode?.ClearOccupant();
            targetNode = null;
        }
    }
}