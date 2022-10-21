using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [RequireComponent(typeof(Interactable))]
    public class Damageable : MonoBehaviour
    {
        private Unit unit;
        private Interactable interactable;

        public Unit Unit => unit;
        public Interactable Interactable => interactable;

        public bool IsAlive => unit.Vitals.Health.CurrentValue > 0f;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            interactable = GetComponent<Interactable>();
            unit.Vitals.Health.Reset();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsAlive)
            {
                Die();
            }
        }

        public void TakeDamage(float amount)
        {
            // TODO - Incorporate armor into calculation

            float damageInflicted = amount * (1 - unit.Attributes.toughness * 0.01f); 
            unit.Vitals.Poise.Reduce(damageInflicted);
            unit.Vitals.Health.Reduce(damageInflicted);
            if (unit.Vitals.Poise.CurrentValue <= 50f)
            {
                unit.AIController.AnimateStagger();
            }
        }

        public void Die()
        {
            unit.AIController.AnimateDeath();

            //TODO - Clean up code to disable unit
            unit.Disable();
        }

        public void ResetHealth()
        {
            gameObject.SetActive(true);
            unit.Vitals.Health.Reset();
        }
    }
}