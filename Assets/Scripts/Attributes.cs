using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public struct Stats
    {
        [Range(0f, 50f)]
        public float armor;
    }

    [System.Serializable]
    public struct Attributes
    {
        [Range(0f, 100f)]
        public float strength;
        
        [Range(0f, 100f)]
        public float dexterity;

        public float attackSpeed => (1 + dexterity * 0.01f);
        
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
    }
}