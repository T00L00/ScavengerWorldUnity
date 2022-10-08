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

        public virtual void Reset()
        {
            useableActions.Clear();
        }

        public virtual void GetUseableActions(Unit unit)
        {
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

        public Action DecideBestAction(Unit unit)
        {
            float highestScore = 0f;
            int nextBestActionIndex = 0;
            for (int i = 0; i < useableActions.Count; i++)
            {
                float actionScore = ScoreAction(useableActions[i], unit);
                if (actionScore > highestScore)
                {
                    nextBestActionIndex = i;
                    highestScore = actionScore;
                }
            }

            if (highestScore == 0)
            {
                return null;
            }

            return useableActions[nextBestActionIndex].Copy();
        }

        private float ScoreAction(UtilityAction action, Unit unit)
        {
            if (!action.IsAchievable(unit))
            {
                //action.Score = 0f;
                return 0f;
            }

            if (action.considerations.Length == 0)
            {
                //action.Score = 0;
                return 0f;
            }
            else
            {
                float score = 1f;
                int considerationCount = action.considerations.Length;
                for (int i = 0; i < considerationCount; i++)
                {
                    float considerationScore = action.considerations[i].ScoreConsideration(unit, action.Target, considerationCount);
                    score *= considerationScore;

                    if (score == 0)
                    {
                        //action.Score = 0;
                        return 0f;
                    }
                }

                //action.Score = score;
                return score;
            }
        }

        #endregion
    }
}