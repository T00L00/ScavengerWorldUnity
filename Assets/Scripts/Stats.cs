using ScavengerWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public struct Stats
    {
        private float baseDamageReduction;
        private float extraDamageReduction;
        private float baseAttackSpeed;
        private float extraAttackSpeed;
        private Attributes attributes;

        public float AttackSpeed => baseAttackSpeed + extraAttackSpeed;
        public float DamageReduction => baseDamageReduction + extraDamageReduction;

        public Stats(Attributes attributes)
        {
            this.attributes = attributes;
            baseDamageReduction = attributes.toughness;
            extraDamageReduction = 0f;
            baseAttackSpeed = 1 + attributes.dexterity * 0.01f;
            extraAttackSpeed = 0f;
        }

        public void AddDamageReduction(float amount)
        {
            extraDamageReduction = Mathf.Clamp(extraDamageReduction + amount, 0f, 200f);
        }

        public void ReduceDamageReduction(float amount)
        {
            extraDamageReduction = Mathf.Clamp(extraDamageReduction - amount * 0.01f, 0f, 200f);
        }

    }
}