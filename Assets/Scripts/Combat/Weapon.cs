using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;

namespace ScavengerWorld
{
    public class Weapon : MonoBehaviour
    {
        public float damageModifier;
        public List<UtilityAction> availableActions;
    }
}