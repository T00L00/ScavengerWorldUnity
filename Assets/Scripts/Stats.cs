using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Stats
    {
        public float attackDamage;
        public float poise;

        [Tooltip("Amount harvested per game-hour")]
        public int gatherRate;
    }
}