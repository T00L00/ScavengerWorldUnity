using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Vital
    {
        [SerializeField] private float baseMax;
        private float currentMax;
        private float currentValue;

        public float CurrentValue => currentValue;
        public float MaxValue => currentMax;
        public float Percentage => currentValue / currentMax;

        public Vital(float currentValue, float baseMax)
        {
            this.currentValue = currentValue;
            this.baseMax = baseMax;
            this.currentMax = baseMax;
        }

        public void Init(float offsetMax)
        {
            currentMax = baseMax + offsetMax;
            currentValue = currentMax;
        }

        public void SetCurrentMax(float amount)
        {
            currentMax = amount;
        }

        public void SetCurrentValue(float amount)
        {
            currentValue = Mathf.Clamp(amount, 0, currentMax);
        }

        public void Reduce(float amount)
        {
            currentValue = Mathf.Clamp(currentValue - amount, 0, currentMax);
        }

        public void Add(float amount)
        {
            currentValue = Mathf.Clamp(currentValue + amount, 0, currentMax);
        }

        public void Reset()
        {
            currentMax = baseMax;
            currentValue = baseMax;
        }
    }
}