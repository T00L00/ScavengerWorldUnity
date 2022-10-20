using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class CharacterAttributes : MonoBehaviour
    {
        [SerializeField] private Attribute health;
        [SerializeField] private Attribute energy;
        [SerializeField] private Attribute poise;

        private Unit unit;

        public Attribute Health => health;
        public Attribute Energy => energy;
        public Attribute Poise => poise;

        private void Awake()
        {
            unit = GetComponent<Unit>();

            health.Reset();
            energy.Reset();
            poise.Reset();
        }

        private void Update()
        {
            if (energy.Percentage < 1f)
            {
                energy.Add(unit.Stats.energyReplenishRate * Time.deltaTime);
            }

            if (poise.CurrentValue <= 0f)
            {
                unit.AIController.AnimateStagger();
            }

            if (poise.Percentage < 1f)
            {
                poise.Add(unit.Stats.poiseReplenishRate * Time.deltaTime);
            }
        }
    }
}