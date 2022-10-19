using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [RequireComponent(typeof(Interactable))]
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private Attribute health;

        private Unit unit;
        private Interactable interactable;

        public Unit Unit => unit;
        public Interactable Interactable => interactable;
        public float CurrentHealth => health.CurrentValue;
        public float HealthPercentage => health.Percentage;

        public bool IsAlive => health.CurrentValue > 0f;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            interactable = GetComponent<Interactable>();
            health.Reset();
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

            health.Reduce(amount);
        }

        public void Die()
        {
            unit.AIController.StopMoving();
            unit.AnimController.AnimateDeath();
        }

        public void ResetHealth()
        {
            gameObject.SetActive(true);
            health.Reset();
        }
    }
}