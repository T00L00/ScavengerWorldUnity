﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public abstract class Entity : MonoBehaviour
    {
        [Range(1, 10_000)]
        [Tooltip("The maximum health for the entity.")]
        public float MaxHealth = 10;

        [HideInInspector]
        public float Health { get; private set; }

        [HideInInspector]
        public bool IsAlive { get => Health > 0; }

        private EntitySummary Summary;

        virtual public void Reset()
        {
            Health = MaxHealth;
            gameObject.SetActive(true);
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (!IsAlive)
                Die();
        }

        /// <summary>
        /// Function that executes whent the entity has been killed/destroyed
        /// </summary>
        private void Die()
        {
            //TODO: Does this entity need to be destroyed or fire off any events?
            gameObject.SetActive(false);
        }

        protected T Pop<T>(List<T> list, int index = -1)
        {
            if (list.Count == 0)
                return default;

            if (index < 0)
                index += list.Count;

            index %= list.Count;
            var element = list[index];
            list.RemoveAt(index);
            return element;
        }

        private void Update()
        {
            Summary = null;
        }

        /// <summary>
        /// Summarize the top visual features in a simple to interpret way.
        /// Convert to a float vector using <see cref="EntitySummary.ToArray"/>
        /// </summary>
        /// <returns></returns>
        public virtual EntitySummary Summarize()
        {
            if (Summary)
                return Summary;

            var renderer = GetComponentInChildren<MeshRenderer>();
            var size = renderer.bounds.size;
            // This function (renderer.material) automatically instantiates the materials and makes them unique to this renderer. 
            //      It is your responsibility to destroy the materials when the game object is being destroyed.
            var color = renderer.material.color;

            Summary = new EntitySummary { Size = size, Color = color, Health = Health };
            return Summary;
        }

        private void OnDestroy()
        {
            var renderer = GetComponent<MeshRenderer>();
            if (renderer)
                Destroy(renderer.material);
        }
    }

    public class EntitySummary
    {
        public Vector3 Size;
        public Color Color;
        public float Health;
        public float Custom1;
        public float Custom2;

        public float[] ToArray()
        {
            return new float[]
            {
                Size.x, Size.y, Size.z, Color.r, Color.g, Color.b, Health, Custom1, Custom2
            };
        }

        public static implicit operator bool(EntitySummary es)
        {
            return es != null;
        }
    }
}