using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScavengerWorld
{
    public class Mover
    {
        private Unit unit;
        private NavMeshAgent navigator;
        private Vector3 facing;

        private Vector3 targetPos;

        public bool MoveEnabled { get; private set; }
        public float Speed => navigator.velocity.magnitude;

        public Mover(Unit unit, NavMeshAgent navigator)
        {
            this.unit = unit;
            this.navigator = navigator;
            facing = unit.transform.forward;
        }

        public void SetMaxSpeed(float speed)
        {
            navigator.speed = speed;
        }

        public bool HasReachedTargetNode(InteractNode node)
        {
            float distance = Vector3.Distance(unit.transform.position, node.transform.position);
            return Mathf.Abs(distance - navigator.stoppingDistance) <= 0.5f;
        }

        public bool HasReachedTarget(Interactable target)
        {
            float distance = Vector3.Distance(unit.transform.position, target.transform.position);
            return Mathf.Abs(distance - target.useRange) <= 0.5f;
        }

        public bool HasReachedTargetPos()
        {
            float distance = Vector3.Distance(unit.transform.position, targetPos);
            return Mathf.Abs(distance - navigator.stoppingDistance) <= 0.5f;
        }

        public void ResetTargetPos()
        {
            StopMoving();
            targetPos = default;            
        }

        public void MoveToTargetNode(InteractNode node)
        {
            if (MoveEnabled)
            {
                navigator.SetDestination(node.transform.position);
            }
        }

        public void MoveToTarget(Interactable target)
        {
            if (MoveEnabled)
            {
                navigator.SetDestination(target.transform.position);
            }
        }

        public void MoveToPosition(Vector3 pos)
        {
            if (MoveEnabled)
            {
                targetPos = pos;
                navigator.SetDestination(pos);
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
            navigator.velocity = Vector3.zero;
            navigator.ResetPath();
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

        public void HandleRotation()
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
    }
}