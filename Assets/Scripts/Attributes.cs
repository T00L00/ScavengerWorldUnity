using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public struct Attributes
    {
        [Range(0f, 100f)]
        [Tooltip("Chance to attack")]
        public float attack;

        [Range(0f, 100f)]
        [Tooltip("Chance to defend")]
        public float defense;

        [Range(0f, 100f)]
        public float strength;
        
        [Range(0f, 100f)]
        public float dexterity;
        
        [Range(0f, 50f)]
        public float toughness;
        
        [Range(0f, 100f)]
        public float will;

        [Tooltip("Poise replenished per second")]
        [Range(0f, 10f)]
        public float poiseRegen;

        [Tooltip("Amount harvested per game-hour")]
        [Range(0f, 100f)]
        public int labouring;

        public float AttackSpeed => (1 + dexterity * 0.01f);
    }
}