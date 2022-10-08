using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI.UAI;
using UnityEngine.Events;
using Google.Protobuf.WellKnownTypes;

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
        private AIState aiState;
        private Action selectedAction;

        public UnityAction<Action> OnDecideAction;

        public AIState AIState => aiState;
        public Action SelectedAction => selectedAction;

        // Start is called before the first frame update
        void Start()
        {
            unit = GetComponent<Unit>();

            aiState = AIState.Default;
            combatAI = new CombatUtilityAI();
            defaultAI = new UtilityAI();
            currentAI = defaultAI;
        }

        // Update is called once per frame
        void Update()
        {
            if (selectedAction is null)
            {
                currentAI.GetUseableActions(unit);
                if (currentAI.useableActions.Count == 0) return;

                selectedAction = currentAI.DecideBestAction(unit);
                if (selectedAction != null)
                {
                    OnDecideAction?.Invoke(SelectedAction);
                }
            }
        }

        public void SetState(AIState state, Interactable target=null)
        {
            switch (state)
            {
                case AIState.Default:
                    selectedAction = null;
                    combatAI.Reset();
                    aiState = state;
                    currentAI = defaultAI;
                    break;
                case AIState.Combat:
                    defaultAI.Reset();
                    aiState = state;
                    combatAI.Target = target;
                    currentAI = combatAI;
                    break;
                default:
                    break;
            }
        }

        public void ClearSelectedAction()
        {
            selectedAction = null;
        }
    }
}