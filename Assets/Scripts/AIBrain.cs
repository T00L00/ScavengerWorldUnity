using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    /// <summary>
    /// Parent class for traditional heuristic AI systems. Logic to decide the next action should be done here.
    /// </summary>
    public class AIBrain : MonoBehaviour, IAIBrain
    {
        public Action SelectedAction { get; private set; }

        public event UnityAction<Action> OnDecideAction;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }



    }
}