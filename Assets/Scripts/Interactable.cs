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
        [SerializeField] private int occupantSpotsCount;
        public float useRange = 1f;
        [Tooltip("Use this list if using RL AI")]
        public List<Action> availableActions;
        [Tooltip("Use this list if using UtilityAI")]
        public List<UtilityAction> availableUtilityActions;

        private Unit unit;
        private Damageable damageable;
        private Gatherable gatherable;
        private OccupantSpot[] occupantSpots;

        public Unit Unit => unit;
        public Damageable Damageable => damageable;
        public Gatherable Gatherable => gatherable;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            damageable = GetComponent<Damageable>();
            gatherable = GetComponent<Gatherable>();
            InitActions();
            InitOccupantSpots();
        }

        private void OnEnable()
        {
            InitOccupantSpots();
        }

        private void InitActions()
        {
            if (availableUtilityActions.Count > 0)
            {
                foreach (UtilityAction a in availableUtilityActions)
                {
                    a.Target = this;
                }
            }
        }

        private void InitOccupantSpots()
        {
            occupantSpots = new OccupantSpot[occupantSpotsCount];
            for (int i = 0; i < occupantSpotsCount; i++)
            {
                occupantSpots[i] = new(
                    this,
                    new Vector3(
                    transform.position.x + useRange * Mathf.Cos(2 * Mathf.PI * ((float)i / occupantSpotsCount)),
                    transform.position.y,
                    transform.position.z + useRange * Mathf.Sin(2 * Mathf.PI * ((float)i / occupantSpotsCount))
                    ));

                //Debug.Log($"{occupantSpots[i].Parent.name} | Occupant Spot {i} | {occupantSpots[i].Position}");
            }
        }

        public bool OccupantSpotAvailable()
        {
            for (int i = 0; i < occupantSpotsCount; i++)
            {
                if (!occupantSpots[i].Occupied)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetOccupantSpot(out OccupantSpot spot)
        {
            spot = null;
            for (int i = 0; i < occupantSpotsCount; i++)
            {
                if (!occupantSpots[i].Occupied)
                {
                    spot = occupantSpots[i];
                    return true;
                }
            }
            return false;
        }

        public OccupantSpot GetAvailableOccupantSpot()
        {
            for (int i = 0; i < occupantSpotsCount; i++)
            {
                if (!occupantSpots[i].Occupied)
                {
                    return occupantSpots[i];                    
                }
            }

            return null;
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