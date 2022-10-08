using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace ScavengerWorld.AI.UAI
{
    public class CombatUtilityAI : UtilityAI
    {
        private readonly Dictionary<Weapon, List<UtilityAction>> weaponActionsRepo = new();

        public Interactable Target { get; set; }

        public CombatUtilityAI()
        {
            
        }

        //public override void Reset()
        //{
        //    useableActions.Clear();
        //    Target = null;
        //}

        //public override void GetUseableActions(Unit unit)
        //{
        //    useableActions.Clear();
        //    useableActions.AddRange(unit.Weapon.attackActions);
        //    foreach (Action a in useableActions)
        //    {
        //        a.Target = this.Target;
        //    }
        //}

        public override void PopulateActionsRepo()
        {
            Weapon[] weapons = GameObject.FindObjectsOfType<Weapon>();
            foreach (Weapon w in weapons)
            {
                weaponActionsRepo[w] = GenerateUtilityActions(w.attackActions);
            }
        }

        public override List<UtilityAction> GetAvailableActions(Unit unit, Interactable combatTarget)
        {
            List<UtilityAction> actions = new();
            actions.AddRange(weaponActionsRepo[unit.Weapon]);
            return actions;
        }
    }
}