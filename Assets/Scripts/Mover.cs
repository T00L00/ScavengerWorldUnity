using System;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
#endif
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
        private Vector3 facing;

        public Interactable Target { get; set; }

        public float Speed => navigator.velocity.magnitude;
        public float StopDistance => navigator.stoppingDistance;

        private void Awake()
        {
            navigator = GetComponent<NavMeshAgent>();
            unit = GetComponent<Unit>();
            navigator.autoRepath = true;
            navigator.updateRotation = false;
        }

        private void Update()
        {
            HandleRotation();
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

        public void Move(Vector3 pos)
        {
            navigator.SetDestination(pos);
        }

        public bool HasReachedTarget(Interactable target) 
        {
            //Debug.Log($"{gameObject.name} Position: {transform.position}");
            return Vector3.Distance(transform.position, target.Unit.Mover.transform.position) <= target.useRange;
        } 

        public void MoveToTarget(Interactable target)
        {
            navigator.SetDestination(target.Unit.Mover.transform.position);
        }

        public void StopMoving()
        {
            navigator.velocity = Vector3.zero;
            navigator.ResetPath();
        }

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
    }
}