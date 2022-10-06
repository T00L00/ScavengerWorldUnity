using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;
using UnityEngine.Events;

namespace ScavengerWorld
{
    public class Weapon : MonoBehaviour
    {
        [Range(1f, 3f)]
        [SerializeField] private float damageModifier = 1f;
        [SerializeField] private Unit unit;
        public List<UtilityAction> attackActions;

        public float DamageModifier => damageModifier;

        //private void OnTriggerEnter(Collider other)
        //{
        //    Unit enemyUnit = other.GetComponent<Mover>()?.Unit;
        //    if (enemyUnit != unit && enemyUnit != null)
        //    {
        //        float totalDamage = unit.Weapon.DamageModifier * unit.Stats.baseDamage;
        //        enemyUnit.Damageable.TakeDamage(totalDamage);
        //        enemyUnit.Attributes.Poise.Reduce(totalDamage * 0.5f);
        //        enemyUnit.Attributes.Energy.Reduce(totalDamage * 0.2f);
        //    }

        //    //Debug.Log($"Sword hit dealing {totalDamage}!");
        //}
    }
}