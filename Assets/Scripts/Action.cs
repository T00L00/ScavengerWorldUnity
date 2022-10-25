using JetBrains.Annotations;
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

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (!(obj is ActionData)) return false;

            ActionData other = (ActionData)obj;
            if (this.unit == other.unit
                && this.target == other.target
                && this.animation == other.animation)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(unit, target, animation);
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

        /// <summary>
        /// Return an exact copy of this action instance.
        /// </summary>
        /// <returns></returns>
        public Action Copy()
        {
            return new Action(actionLogic, data);
        }

        /// <summary>
        /// Return a copy of this action instance with the provided unit and animation data.
        /// Target from current instance will be carried over.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="animation"></param>
        /// <returns></returns>
        public Action Copy(Unit unit, AnimationClip animation)
        {
            return new Action(actionLogic, new ActionData(unit, data.target, animation));
        }

        /// <summary>
        /// Return a copy of this action instance with the provided unit, target, and animation data.
        /// Use primarily to copy actions that have not been initialized with a target.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="target"></param>
        /// <param name="animation"></param>
        /// <returns></returns>
        public Action Copy(Unit unit, Interactable target, AnimationClip animation)
        {
            return new Action(actionLogic, new ActionData(unit, target, animation));
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (!(obj is Action)) return false;

            Action other = obj as Action;
            if (this.actionLogic == other.GetActionLogic()
                && this.data.unit == other.data.unit)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(actionLogic, data);
        }
    }
}