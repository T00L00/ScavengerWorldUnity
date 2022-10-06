using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Events;
using ScavengerWorld.AI;
using UnityEngine.UIElements;

namespace ScavengerWorld.AI.UAI
{
    public class UtilityAI
    {
        public readonly List<UtilityAction> useableActions = new();

        public UtilityAI()
        {
            
        }

        public virtual void GetUseableActions(Unit unit)
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

        #region Decide best action

        public UtilityAction DecideBestAction(Unit unit)
        {
            UtilityAction selectedAction = null;

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
                return null;
            }

            selectedAction = useableActions[nextBestActionIndex];
            //Debug.Log($"Best action: {selectedAction.Name}");
            //Debug.Log($"Best action: {selectedAction.Name}");
            //return selectedAction;
            return selectedAction;
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