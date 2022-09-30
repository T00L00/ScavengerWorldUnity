using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public abstract class ActionLogic : ScriptableObject
    {
        //public ActionType actionType;

        [TextArea(1,5)]
        public string description;

        public static ActionLogic Load(string name)
        {
            return Resources.Load<ActionLogic>($"ActionLogics/{name}");
        }

        public abstract bool RequiresInRange(Unit unit, Interactable target);

        public abstract bool IsAchievable(Unit unit, Interactable target);

        /// <summary>
        /// Logic to run before time-dependent logic runs. Good for checking things
        /// or executing instantaneous logic.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public abstract void StartAction(Unit unit, Interactable target);

        /// <summary>
        /// Time-dependent action logic goes here. Ex: gathering over time
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public abstract void UpdateAction(Unit unit, Interactable target);

        /// <summary>
        /// Logic to run after time-dependent logic has run. Good for any cleanup
        /// or post-action checks.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public abstract void StopAction(Unit unit, Interactable target);
    }
}