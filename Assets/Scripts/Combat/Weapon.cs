using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;
using UnityEngine.Events;
using Animancer;

namespace ScavengerWorld
{
    public class Weapon : MonoBehaviour
    {
        [Range(1f, 3f)]
        [SerializeField] private float damageModifier = 1f;
        [SerializeField] private Unit unit;
        [SerializeField] private AnimationClip[] attackAnimations;
        [SerializeField] private AnimationClip[] defendAnimations;

        public LinearMixerTransition combatLocomotion;

        private BoxCollider weaponCollider;

        public float DamageModifier => damageModifier;

        private void Awake()
        {
            weaponCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Unit enemyUnit = other.GetComponent<Unit>();
            if (enemyUnit != null && enemyUnit != unit && enemyUnit.TeamId != unit.TeamId) //&& rb.velocity.magnitude > 1)
            {
                if (enemyUnit.AIController.CombatState.IsBlocking)
                {
                    //unit.AIController.AnimateStagger();
                    return;
                }

                float totalDamage = damageModifier * unit.Attributes.strength;
                enemyUnit.Damageable.TakeDamage(totalDamage);
            }

            //Debug.Log($"Sword hit dealing {totalDamage}!");
        }

        public AnimationClip RandomAttackAnimation()
        {
            return attackAnimations[Random.Range(0, attackAnimations.Length)];
        }

        public AnimationClip RandomDefendAnimation()
        {
            return defendAnimations[Random.Range(0, defendAnimations.Length)];
        }

        public void Disable()
        {
            weaponCollider.enabled = false;
        }
    }
}