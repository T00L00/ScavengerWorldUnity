using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public struct Stats
    {
        public float baseDamage;
        public float attackSpeed;
        [Tooltip("Energy replenished per sec")]
        public float energyReplenishRate;
        [Tooltip("Poise replenished per sec")]
        public float poiseReplenishRate;
        [Tooltip("Amount harvested per game-hour")]
        public int gatherRate;
    }
}