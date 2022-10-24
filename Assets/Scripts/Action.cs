using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace ScavengerWorld
{
    public struct ActionData
    {
        public Unit unit;
        public Interactable target;
        public AnimationClip animation;

        public ActionData(Unit unit, Interactable target, AnimationClip animation)
        {
            this.unit = unit;
            this.target = target;
            this.animation = animation;
        }
    }

    /// <summary>
    /// Wrapper class that allows for creating available actions on Unit in inspector
    /// </summary>
    [System.Serializable]
    public class Action
    {
        [SerializeField] protected ActionLogic actionLogic;

        protected ActionData data;

        // Constructor should only be used when making a copy of the original action. 
        // By using the constructor, you preload the action with all the necessary data to run it.
        public Action(ActionLogic actionLogic, ActionData data)
        {
            this.actionLogic = actionLogic;
            this.data = data;
        }

        public string Name => actionLogic.name;

        public virtual bool IsEmpty => actionLogic is null;

        public Interactable Target 
        {
            get => data.target;
            set
            {
                data.target = value;
            }
        }

        public bool IsRunning { get; set; }

        public void Init(ActionData data)
        {
            this.data = data;
        }

        public ActionLogic GetActionLogic() => actionLogic;

        public bool IsAchievable(Unit unit) => actionLogic.IsAchievable(unit, Target);

        public bool RequiresInRange() => actionLogic.RequiresInRange(data);

        public void StartAction()
        {
            IsRunning = true;
            actionLogic.StartAction(data);
        }

        public void UpdateAction()
        {
            actionLogic.UpdateAction(data);
        }

        public void StopAction()
        {
            IsRunning = false;
            actionLogic.StopAction(data);
        }

        // Only need unit and animation because original Action instance being copied
        // already holds reference to target interactable

        // Use for actions that get initialized with a target
        public Action Copy(Unit unit, AnimationClip animation)
        {
            return new Action(actionLogic, new ActionData(unit, data.target, animation));
        }

        // Use for actions that don't get initialized with a target
        public Action Copy(Unit unit, Interactable target, AnimationClip animation)
        {
            return new Action(actionLogic, new ActionData(unit, target, animation));
        }
    }
}