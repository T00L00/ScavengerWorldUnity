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
        public Interactable Target { get; set; }

        private readonly List<UtilityAction> interactableActions = new();

        public UtilityAI()
        {
            
        }

        public virtual void Reset()
        {
            interactableActions.Clear();
        }

        public void GetInteractableActions(Unit unit)
        {
            interactableActions.Clear();
            List<Interactable> interactables = unit.Pulse();
            if (interactables.Count > 0)
            {
                foreach (Interactable i in interactables)
                {
                    interactableActions.AddRange(i.InteractableActions);
                }
            }
        }

        #region Decide best action

        public virtual Action DecideBestAction(Unit unit)
        {
            Action action = null;
            action = SelectInteractableAction(unit);
            if (action is null)
            {
                action = SelectDefaultAction(unit);
            }
            return action;
        }

        private Action SelectInteractableAction(Unit unit)
        {
            float highestScore = 0f;
            int nextBestActionIndex = 0;
            for (int i = 0; i < interactableActions.Count; i++)
            {
                float actionScore = ScoreAction(interactableActions[i], unit);
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

            return interactableActions[nextBestActionIndex].Copy(unit, null);
        }

        private Action SelectDefaultAction(Unit unit)
        {
            return unit.DefaultActions[0]?.Copy(unit, null);
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