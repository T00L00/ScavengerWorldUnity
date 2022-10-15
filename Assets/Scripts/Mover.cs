using System;
using System.Collections.Generic;
using System.Xml;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScavengerWorld
{
    /// <summary>
    /// Responsible for handling all movement related logic
    /// </summary>
    public class Mover : MonoBehaviour
    {        
        [Range(5f, 100f)]
        [SerializeField] private float focusRange = 100f;
        [Range(20f, 50f)]
        [Tooltip("Max range from (0,0,0) to alternatively explore if AI told agent to go off the map")]
        [SerializeField] private float explorationRange = 40f;
        [Range(10f, 30f)]
        [SerializeField] private float rotateSpeed = 20f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private NavMeshAgent navigator;
                
        private Unit unit;
        private ActionRunner actionRunner;
        private Vector3 facing;
        private InteractNode targetSpot;

        public event UnityAction OnReachedDestination;

        public Unit Unit => unit;
        public float NavigatorSpeed => navigator.velocity.magnitude;
        public float StopDistance => navigator.stoppingDistance;

        public bool MoveEnabled { get; private set; }

        public InteractNode TargetSpot => targetSpot;

        public Interactable TargetInteractable { get; set; }

        private void Awake()
        {
            unit = GetComponent<Unit>();
            actionRunner = GetComponent<ActionRunner>();
            navigator = GetComponent<NavMeshAgent>();
            navigator.autoRepath = true;
            navigator.updateRotation = false;
            
            MoveEnabled = true;
            TargetInteractable = null;
        }

        void Update()
        {
            HandleRotation();

            if (TargetInteractable != null)
            {
                if (targetSpot != null)
                {
                    if (HasReachedTargetSpot())
                    {
                        if (!actionRunner.ActionIsRunning)
                        {
                            StopMoving();
                            OnReachedDestination?.Invoke();
                        }                        
                    }
                    else
                    {
                        MoveToTargetSpot();
                    }
                }
                else
                {
                    if (TargetInteractable.InteractionAvailable)
                    {
                        targetSpot = TargetInteractable.GetAvailableInteractNode(this.unit);
                        if (targetSpot != null)
                        {
                            targetSpot.SetOccupant(unit);
                        }
                        else
                        {
                            actionRunner.CancelCurrentAction();
                        }
                    }
                    else
                    {
                        actionRunner.CancelCurrentAction();
                    }                    
                }
            }

            //if (TargetInteractable is null && TargetPos != default)
            //{
            //    if (HasReachedTargetPos())
            //    {
            //        StopMoving();
            //        targetPos = default;
            //        return;
            //    }
            //    return;
            //}

            //if (TargetInteractable != null && TargetPos == default)
            //{
            //    if (HasReachedTargetInteractable())
            //    {
            //        if (!actionRunner.ActionIsRunning)
            //        {
            //            StopMoving();
            //            OnReachedInteractableTarget?.Invoke();
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        //if (!actionRunner.ActionIsRunning)
            //        //{
            //        //    MoveToInteractable(TargetInteractable);
            //        //}
            //        MoveToInteractable(TargetInteractable);
            //    }

            //    // TODO: Need logic here to tell agent to recalculate new path if stuck
            //}
        }

        public bool HasReachedTargetSpot()
        {
            float distance = Vector3.Distance(transform.position, targetSpot.transform.position);
            return Mathf.Abs(distance - navigator.stoppingDistance) <= 0.01;
        }

        /// <summary>
        /// Use this to move character to clicked position
        /// </summary>
        /// <param name="pos"></param>
        public void MoveToPosition(Vector3 pos)
        {
            if (MoveEnabled)
            {
                actionRunner.CancelCurrentAction();
                navigator.SetDestination(pos);
            }            
        }

        public void MoveToTargetSpot()
        {
            if (MoveEnabled)
            {
                navigator.SetDestination(targetSpot.transform.position);
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

        public void ClearTarget()
        {
            targetSpot?.ClearOccupant();
            targetSpot = null;
            TargetInteractable = null;
        }

        #region Old Stuff

        public bool FoodIsNearby(out Gatherable gatherable)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, focusRange, interactableLayer);

            if (colliders.Length == 0) 
            {
                gatherable = null;
                return false;
            };

            foreach (Collider c in colliders)
            {
                gatherable = c.GetComponent<Gatherable>();
                if (gatherable != null) return true;
            }
            gatherable=null;
            return false;
        }

        public bool EnemyUnitIsNearby(out Unit enemyUnit)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, focusRange, interactableLayer);

            if (colliders.Length == 0)
            {
                enemyUnit = null;
                return false;
            }                

            foreach (Collider c in colliders)
            {
                enemyUnit = c.GetComponent<Unit>();
                if (enemyUnit != null
                    && enemyUnit.TeamId != this.unit.TeamId
                    && !enemyUnit.IsStorageDepot)
                {
                    return true;
                }
            }
            enemyUnit=null;
            return false;
        }

        public bool EnemyStorageIsNearby(out Unit enemyStorage)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, focusRange, interactableLayer);

            if (colliders.Length == 0)
            {
                enemyStorage = null;
                return false;
            }

            foreach (Collider c in colliders)
            {
                enemyStorage = c.GetComponent<Unit>();
                if (enemyStorage != null
                    && enemyStorage.TeamId != this.unit.TeamId
                    && enemyStorage.IsStorageDepot)
                {
                    return true;
                }
            }
            enemyStorage=null;
            return false;
        }

        public bool IsValidPath(Vector3 pos)
        {
            NavMeshPath path = new();
            navigator.CalculatePath(pos, path);
            return path.status == NavMeshPathStatus.PathComplete;
        }

        public Vector3 GetValidPath(Vector3 currentPos)
        {
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 20f + Vector3.zero;
            NavMesh.SamplePosition(randomPos, out NavMeshHit hit, explorationRange, NavMesh.AllAreas);
            return hit.position;
        }

        public void ResetNavigator()
        {
            navigator.ResetPath();
        }

        #endregion
    }
}