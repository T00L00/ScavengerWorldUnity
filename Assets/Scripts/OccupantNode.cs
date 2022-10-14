using ScavengerWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class OccupantSpot
    {
        private Interactable parent;
        private Unit occupant;
        private Vector3 position;

        public Interactable Parent => parent;
        public Unit Occupant => occupant;
        public Vector3 Position => position;
        public bool Occupied => occupant != null;

        public OccupantSpot(Interactable parent, Vector3 position)
        {
            this.position = position;
            this.parent = parent;
            this.occupant = null;
        }

        public bool TrySetOccupant(Unit unit)
        {
            if (occupant is null)
            {
                occupant = unit;
                return true;
            }

            if (occupant == unit)
            {
                return true;
            }

            // occupant spot already occupied by someone else
            return false;
        }

        public void SetOccupant(Unit unit)
        {
            if (occupant is null)
            {
                occupant = unit;
            }
        }

        public void ClearOccupant()
        {
            occupant = null;
        }
    }
}