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
        [SerializeField] private WeaponData data;
        private BoxCollider weaponCollider;

        public float Damage => data.damage;
        public float DamageReduction => data.damageReduction;
        public LinearMixerTransition CombatLocomotion => data.locomotion;

        private void Awake()
        {
            weaponCollider = GetComponent<BoxCollider>();
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    Unit enemyUnit = other.GetComponent<Unit>();
        //    if (enemyUnit != null && enemyUnit != unit && enemyUnit.TeamId != unit.TeamId) //&& rb.velocity.magnitude > 1)
        //    {
        //        if (enemyUnit.AIController.CombatState.IsBlocking)
        //        {
        //            unit.AIController.AnimateStagger();
        //            return;
        //        }

        //        float totalDamage = damageModifier * unit.Attributes.strength;
        //        enemyUnit.Damageable.TakeDamage(totalDamage);
        //    }

        //    //Debug.Log($"Sword hit dealing {totalDamage}!");
        //}

        public AnimationClip RandomAttackAnimation()
        {
            return data.attackAnimations[Random.Range(0, data.attackAnimations.Length)];
        }

        public AnimationClip RandomDefendAnimation()
        {
            return data.defendAnimations[Random.Range(0, data.defendAnimations.Length)];
        }

        public void Disable()
        {
            //weaponCollider.enabled = false;
        }        
    }
}