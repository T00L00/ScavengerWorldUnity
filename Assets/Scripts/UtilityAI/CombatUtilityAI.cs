using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace ScavengerWorld.AI.UAI
{
    public class CombatUtilityAI
    {
        private List<Interactable> enemyCombatants;

        public Interactable Target { get; private set; }

        public CombatUtilityAI()
        {
            enemyCombatants = new();
        }

        public void Reset()
        {
            enemyCombatants.Clear();
            Target = null;
        }

        public void AddTarget(Interactable target)
        {
            if (enemyCombatants.Contains(target)) return;

            enemyCombatants.Add(target);
        }

        public Action DecideBestAction(Unit unit)
        {
            // Decide which enemy to fight
            // Decide whether to attack or defend
            // Decide which specific attack or defense action
            // Decide which animation to play

            SelectTarget(unit);

            Action combatAction = null;
            string mode = SelectCombatAction(unit);
            if (mode.Equals(CombatActionType.Attack.ToString()))
            {
                combatAction = unit
                    .AttackActions[Random.Range(0, unit.AttackActions.Count)]
                    .Copy(unit, Target, unit.AttackWeapon.RandomAttackAnimation()); // Hardcoding to use attack weapon for now
            }
            else
            {
                combatAction = unit
                    .DefendActions[Random.Range(0, unit.DefendActions.Count)]
                    .Copy(unit, Target, unit.DefenseWeapon.RandomDefendAnimation()); // Hardcoding to use defense weapon for now
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

        public void SelectTarget(Unit unit)
        {
            Target = enemyCombatants[Random.Range(0, enemyCombatants.Count)];
            Target.Unit.AIController.CombatState.AddTarget(unit.Interactable);
        }
    }
}