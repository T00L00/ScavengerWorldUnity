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
        private AIModule aiModule;
        private AIState aiState;
        private Action selectedAction;
        private Interactable combatTarget;

        public UnityAction<Action> OnDecideAction;

        public AIState AIState => aiState;
        public Action SelectedAction => selectedAction;

        // Start is called before the first frame update
        void Start()
        {
            unit = GetComponent<Unit>();

            aiState = AIState.Default;
            aiModule = ServiceLocator.instance.GetService<AIModule>();
        }

        // Update is called once per frame
        void Update()
        {
            if (selectedAction is null)
            {
                selectedAction = aiModule.DecideAction(aiState, unit, combatTarget);
                
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
                    aiState = state;
                    combatTarget = null;
                    break;
                case AIState.Combat:
                    aiState = state;
                    combatTarget = target;
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