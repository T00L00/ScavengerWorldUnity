using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;

namespace ScavengerWorld
{
    public class Weapon : MonoBehaviour
    {
        [Range(1f, 3f)]
        [SerializeField] private float damageModifier = 1f;
        public List<UtilityAction> attackActions;
    }
}