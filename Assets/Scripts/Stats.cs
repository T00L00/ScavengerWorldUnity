using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Stats
    {
        public int attackDamage;

        [Tooltip("Amount harvested per game-hour")]
        public int gatherRate;
    }
}