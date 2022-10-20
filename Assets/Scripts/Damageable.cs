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
        private CharacterAttributes attributes;

        public Unit Unit => unit;
        public Interactable Interactable => interactable;
        public float CurrentHealth => attributes.Health.CurrentValue;
        public float HealthPercentage => attributes.Health.Percentage;

        public bool IsAlive => attributes.Health.CurrentValue > 0f;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            interactable = GetComponent<Interactable>();
            attributes = GetComponent<CharacterAttributes>();
            attributes.Health.Reset();
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
            // Perform damage reduction calculation based on armor

            attributes.Health.Reduce(amount);
        }

        public void Die()
        {
            unit.AIController.StopMoving();
            unit.AIController.AnimateDeath();

            //TODO - Clean up code to disable unit
        }

        public void ResetHealth()
        {
            gameObject.SetActive(true);
            attributes.Health.Reset();
        }
    }
}