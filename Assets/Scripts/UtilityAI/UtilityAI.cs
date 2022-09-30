using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI.UtilityAI
{
    public class UtilityAI : AIBrain
    {
        private Unit unit;

        private UtilityAction selectedAction;
        private List<UtilityAction> useableActions = new();

        public override Action SelectedAction => selectedAction;

        void Awake()
        {
            unit = GetComponent<Unit>();
        }

        // Update is called once per frame
        void Update()
        {
            if (selectedAction is null)
            {
                GetUseableActions();
                if (useableActions.Count == 0) return;

                DecideBestAction(useableActions, unit);                
            }
        }

        private void GetUseableActions()
        {
            useableActions.Clear();
            List<Interactable> interactables = unit.Pulse();
            if (interactables.Count > 0)
            {
                foreach (Interactable i in interactables)
                {
                    useableActions.AddRange(i.availableUtilityActions);
                }
            }
        }

        public override void ClearCurrentAction()
        {
            selectedAction = null;
        }

        #region Decide best action

        private void DecideBestAction(List<UtilityAction> useableActions, Unit unit)
        {
            float highestScore = 0f;
            int nextBestActionIndex = 0;
            for (int i = 0; i < useableActions.Count; i++)
            {
                ScoreAction(useableActions[i], unit);
                if (useableActions[i].Score > highestScore)
                {
                    nextBestActionIndex = i;
                    highestScore = useableActions[i].Score;
                }
            }

            if (highestScore == 0)
            {
                return;
            }

            if (selectedAction is null)
            {
                selectedAction = useableActions[nextBestActionIndex];
            }
            else if (useableActions[nextBestActionIndex].Name != SelectedAction.Name)
            {
                if (selectedAction.weight <= useableActions[nextBestActionIndex].Score / selectedAction.Score)
                {
                    selectedAction = useableActions[nextBestActionIndex];
                }
            }

            Debug.Log($"Best action: {selectedAction.Name}");

            OnDecideAction?.Invoke(SelectedAction);
        }

        private void ScoreAction(UtilityAction action, Unit unit)
        {
            if (!action.IsAchievable(unit))
            {
                action.Score = 0f;
                return;
            }

            if (action.considerations.Length == 0)
            {
                action.Score = 0;
                return;
            }
            else
            {
                float score = 1f;
                int considerationCount = action.considerations.Length;
                for (int i = 0; i < considerationCount; i++)
                {
                    action.considerations[i].ScoreConsideration(unit, action.Target, considerationCount);
                    score *= action.considerations[i].Score;

                    if (score == 0)
                    {
                        action.Score = 0;
                        return;
                    }
                }
                
                action.Score = score;
            }
        }

        #endregion
    }
}