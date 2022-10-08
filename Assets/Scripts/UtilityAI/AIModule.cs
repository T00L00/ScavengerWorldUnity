using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI.UAI
{
    public class AIModule : MonoBehaviour
    {
        // Consider loading action data from prefab/scriptable objects instead of gameobjects

        private readonly UtilityAI defaultAI = new();
        private readonly CombatUtilityAI combatAI = new();

        private void Start()
        {
            defaultAI.PopulateActionsRepo();
            combatAI.PopulateActionsRepo();
        }

        // Need a way to update actions repositories when new interactables are Instantiated/Destroyed

        public Action DecideAction(AIState state, Unit unit, Interactable combatTarget=null)
        {            
            if (state == AIState.Default)
            {
                return defaultAI.DecideAction(unit);
            }

            if (state == AIState.Combat)
            {
                return combatAI.DecideAction(unit, combatTarget);
            }

            Debug.LogError("Invalid AIState");
            return null;
        }
    }
}