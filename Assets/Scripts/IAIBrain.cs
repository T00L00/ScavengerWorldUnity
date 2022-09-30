using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    public interface IAIBrain
    {
        public Action SelectedAction { get; }
    }
}