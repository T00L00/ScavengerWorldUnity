using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class CharacterVitals : MonoBehaviour
    {
        [SerializeField] private Vital health;
        [SerializeField] private Vital poise;

        private Unit unit;

        public Vital Health => health;
        public Vital Poise => poise;

        private void Awake()
        {
            unit = GetComponent<Unit>();

            health.Init(unit.Attributes.will);
            poise.Init(unit.Attributes.toughness);
        }

        private void Update()
        {
            if (poise.Percentage < 1f)
            {
                poise.Add(unit.Attributes.poiseRegen * Time.deltaTime);
            }
        }
    }
}