using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Combat/New Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;

        [TextArea(1, 5)]
        public string description;

        [Range(0f, 100f)]
        public float damage;
        
        [Range(0f, 100f)]
        public float damageReduction;
        
        public LinearMixerTransition locomotion;
        public AnimationClip[] attackAnimations;
        public AnimationClip[] defendAnimations;
    }
}