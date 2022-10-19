using ScavengerWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class InteractNode : MonoBehaviour
    {
        private Interactable parent;
        private Unit occupant;

        public bool Occupied => occupant != null;

        public void Init(Interactable parent)
        {
            this.parent = parent;
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
                parent.AddInteraction();
            }
        }

        public void ClearOccupant()
        {
            occupant = null;
            parent.RemoveInteraction();
        }
    }
}