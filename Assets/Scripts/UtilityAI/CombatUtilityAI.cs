using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    public class CombatUtilityAI : UtilityAI
    {
        public CombatUtilityAI()
        {
            
        }

        public override void Reset()
        {
            useableActions.Clear();
            Target = null;
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