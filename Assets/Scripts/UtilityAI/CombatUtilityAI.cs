using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace ScavengerWorld.AI.UAI
{
    public class CombatUtilityAI : UtilityAI
    {
        public CombatUtilityAI()
        {
            
        }

        public override void Reset()
        {
            interactableActions.Clear();
            Target = null;
        }

        public override Action DecideBestAction(Unit unit)
        {
            // Decide whether to attack or defend
            // Decide which specific attack or defense action
            // Decide which animation to play

            Action combatAction = null;
            string mode = SelectCombatAction(unit);
            if (mode.Equals(CombatActionType.Attack.ToString()))
            {
                combatAction = unit
                    .AttackActions[Random.Range(0, unit.AttackActions.Count)]
                    .Copy(unit, Target, unit.Weapon.RandomAttackAnimation());
            }
            else
            {
                combatAction = unit
                    .DefendActions[Random.Range(0, unit.DefendActions.Count)]
                    .Copy(unit, Target, unit.Weapon.RandomDefendAnimation());
            }

            return combatAction;
        }

        private string SelectCombatAction(Unit unit)
        {
            System.Random rnd = new();
            string[] choices = { CombatActionType.Attack.ToString(), CombatActionType.Defend.ToString() };
            float[] weights = { unit.Attributes.attack, unit.Attributes.defense };
            string choice = Utils.Sample(rnd, choices, weights);

            return choice;
        }
    }
}