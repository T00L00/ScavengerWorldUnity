using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    public enum AIState
    {
        Default,
        Combat
    }

    public class AIController : MonoBehaviour
    {
        private Unit unit;
        private CombatUtilityAI combatAI;
        private UtilityAI defaultAI;
        private UtilityAI currentAI;

        public UnityAction<Action> OnDecideAction;

        public Action SelectedAction { get; private set; }

        public bool InCombat { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            unit = GetComponent<Unit>();
            combatAI = new CombatUtilityAI();
            defaultAI = new UtilityAI();
            currentAI = defaultAI;
        }

        // Update is called once per frame
        void Update()
        {
            if (SelectedAction is null)
            {
                currentAI.GetUseableActions(unit);
                if (currentAI.useableActions.Count == 0) return;

                SelectedAction = currentAI.DecideBestAction(unit);
                if (SelectedAction != null)
                {
                    OnDecideAction?.Invoke(SelectedAction);
                }
            }
        }

        public void SetState(AIState state)
        {
            switch (state)
            {
                case AIState.Default:
                    currentAI = defaultAI;
                    break;
                case AIState.Combat:
                    currentAI = combatAI;
                    break;
                default:
                    break;
            }
        }

        public void ClearSelectedAction()
        {
            SelectedAction = null;
        }
    }
}