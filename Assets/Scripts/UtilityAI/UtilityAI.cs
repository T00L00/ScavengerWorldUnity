using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI;

namespace ScavengerWorld.AI.UAI
{
    public class UtilityAI
    {
        private readonly Dictionary<Interactable, List<UtilityAction>> interactableActionsRepo = new();

        public UtilityAI()
        {
            
        }

        /// <summary>
        /// Get all the available actions from the scene
        /// </summary>
        public virtual void PopulateActionsRepo()
        {
            Interactable[] interactables = GameObject.FindObjectsOfType<Interactable>();
            foreach (Interactable i in interactables)
            {
                interactableActionsRepo[i] = GenerateUtilityActions(i.availableUtilityActions);
            }
        }

        /// <summary>
        /// Generate UtilityActions from SerializedUtilityActions
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected List<UtilityAction> GenerateUtilityActions(List<SerializedUtilityAction> data)
        {
            List<UtilityAction> actions = new();
            foreach (SerializedUtilityAction a in data)
            {
                actions.Add(a.ToUtilityAction());
            }
            return actions;
        }

        /// <summary>
        /// Get the available actions near the unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual List<UtilityAction> GetAvailableActions(Unit unit, Interactable target=null)
        {
            List<UtilityAction> actions = new();
            List<Interactable> interactables = unit.Pulse();
            foreach (Interactable i in interactables)
            {
                actions.AddRange(interactableActionsRepo[i]);
            }
            return actions;
        }

        #region Decide best action

        public Action DecideAction(Unit unit, Interactable target=null)
        {
            Action selectedAction = null;
            List<UtilityAction> actions = GetAvailableActions(unit, target);

            float highestScore = 0f;
            int nextBestActionIndex = 0;
            for (int i = 0; i < actions.Count; i++)
            {
                float actionScore = ScoreAction(actions[i], unit);
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

            selectedAction = actions[nextBestActionIndex].Copy();
            return selectedAction;
        }

        protected float ScoreAction(UtilityAction action, Unit unit)
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
                float actionScore = 1f;
                int considerationCount = action.considerations.Length;
                for (int i = 0; i < considerationCount; i++)
                {
                    float considerationScore = action.considerations[i].ScoreConsideration(unit, action.Target, considerationCount);
                    actionScore *= considerationScore;

                    if (actionScore == 0)
                    {
                        //action.Score = 0;
                        return 0f;
                    }
                }

                //action.Score = actionScore;
                return actionScore;
            }
        }

        #endregion
    }
}