using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ScavengerWorld.AI.UAI;

namespace ScavengerWorld
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractNode interactNodePrefab;
        [Tooltip("All interact nodes around interactable")]
        [SerializeField] private int possibleInteractNodeCount = 1;
        [Tooltip("Max number of units that can interact with this interactable at one time")]
        [SerializeField] private int maxInteractions = 1;

        public float useRange = 1f;
        //[Tooltip("Use this list if using RL AI")]
        //public List<Action> availableActions;
        [Tooltip("Use this list if using UtilityAI")]
        [SerializeField] private List<UtilityAction> interactableActions;

        private Unit unit;
        private Damageable damageable;
        private Gatherable gatherable;
        private InteractNode[] interactNodes;
        private int interactionCount;

        public ReadOnlyCollection<UtilityAction> InteractableActions => interactableActions.AsReadOnly();
        public Unit Unit => unit;
        public Damageable Damageable => damageable;
        public Gatherable Gatherable => gatherable;

        public bool InteractionAvailable => interactionCount < maxInteractions;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            damageable = GetComponent<Damageable>();
            gatherable = GetComponent<Gatherable>();
            InitActions();
            InitInteractNodes();
            interactionCount = 0;
        }

        //private void OnEnable()
        //{
        //    InitInteractNodes();
        //}

        private void InitActions()
        {
            if (interactableActions.Count > 0)
            {
                foreach (UtilityAction a in interactableActions)
                {
                    a.Init(new ActionData(null, this, null));
                }
            }
        }

        private void InitInteractNodes()
        {
            interactNodes = new InteractNode[possibleInteractNodeCount];
            for (int i = 0; i < possibleInteractNodeCount; i++)
            {
                interactNodes[i] = Instantiate(
                    interactNodePrefab,
                    new Vector3(
                    transform.localPosition.x + useRange * Mathf.Cos(2 * Mathf.PI * ((float)i / possibleInteractNodeCount)),
                    0,
                    transform.localPosition.z + useRange * Mathf.Sin(2 * Mathf.PI * ((float)i / possibleInteractNodeCount))
                    ),
                    Quaternion.identity,
                    this.transform);

                interactNodes[i].Init(this);
                //Debug.Log($"{occupantSpots[i].Parent.name} | Occupant Spot {i} | {occupantSpots[i].Position}");
            }
        }

        public void AddInteraction()
        {
            interactionCount = Mathf.Clamp(interactionCount+1, 0, maxInteractions);
        }

        public void RemoveInteraction()
        {
            interactionCount = Mathf.Clamp(interactionCount-1, 0, maxInteractions);
        }

        public bool InteractNodeAvailable()
        {
            if (!InteractionAvailable)
            {
                return false;
            }

            for (int i = 0; i < possibleInteractNodeCount; i++)
            {
                if (interactionCount < maxInteractions && !interactNodes[i].Occupied)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetInteractNode(out InteractNode node)
        {
            node = null;
            if (InteractionAvailable)
            {
                return false;
            }
            
            for (int i = 0; i < possibleInteractNodeCount; i++)
            {
                if (!interactNodes[i].Occupied)
                {
                    node = interactNodes[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get nearest available interact node
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public InteractNode GetAvailableInteractNode(Unit unit)
        {
            InteractNode nearestNode = null;
            float nearestDist = Mathf.Infinity;
            for (int i = 0; i < possibleInteractNodeCount; i++)
            {
                if (!interactNodes[i].Occupied)
                {
                    float dist = Vector3.Distance(unit.transform.position, interactNodes[i].transform.position);
                    if (dist < nearestDist)
                    {
                        nearestDist = dist;
                        nearestNode = interactNodes[i];
                    } 
                }
            }

            return nearestNode;
        }

        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    foreach (OccupantSpot o in occupantSpots)
        //    {
        //        Gizmos.DrawWireSphere(o.Position, 0.2f);
        //    }
        //}
    }
}