using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    public class CombatUtilityAI : UtilityAI
    {
        public Interactable Target { get; set; }

        public CombatUtilityAI()
        {
            
        }

        public override void GetUseableActions(Unit unit)
        {
            useableActions.Clear();
            useableActions.AddRange(unit.Weapon.attackActions);
            foreach (Action a in useableActions)
            {
                a.Target = this.Target;
            }
        }
    }
}