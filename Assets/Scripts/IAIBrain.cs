using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    public interface IAIBrain
    {
        public event UnityAction<Action> OnDecideAction;

        public Action SelectedAction { get; }
    }
}