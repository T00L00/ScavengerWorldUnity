using ScavengerWorld.AI.UAI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace ScavengerWorld
{
    public class Interactable : MonoBehaviour
    {
        public float useRange = 1f;
        [Tooltip("Use this list if using RL AI")]
        public List<Action> availableActions;
        [Tooltip("Use this list if using UtilityAI")]
        public List<SerializedUtilityAction> availableUtilityActions;


        private Unit unit;
        private Damageable damageable;
        private Gatherable gatherable;

        public Unit Unit => unit;
        public Damageable Damageable => damageable;
        public Gatherable Gatherable => gatherable;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            damageable = GetComponent<Damageable>();
            gatherable = GetComponent<Gatherable>();
            InitActions();
        }

        private void InitActions()
        {
            if (availableUtilityActions.Count > 0)
            {
                foreach (SerializedUtilityAction a in availableUtilityActions)
                {
                    a.SetTarget(this);
                }
            }
        }
    }
}