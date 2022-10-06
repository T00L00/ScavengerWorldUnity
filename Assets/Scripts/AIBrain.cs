using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    /// <summary>
    /// Parent class for traditional heuristic AI systems. Logic to decide the next action should be done here.
    /// </summary>
    public class AIBrain : IAIBrain
    {
        public virtual Action SelectedAction { get; private set; }

        public UnityAction<Action> OnDecideAction;

        public virtual void ClearCurrentAction() { }
    }
}