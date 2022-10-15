using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Sensor
    {
        [SerializeField] private float radius;

        public float Radius => radius;

        public Sensor(float radius)
        {
            this.radius = radius;
        }

        public List<Interactable> Pulse(Vector3 center)
        {
            List<Interactable> interactables = new();
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            foreach (Collider c in hitColliders)
            {
                Interactable interactable = c.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactables.Add(interactable);
                }
            }

            return interactables;
        }
    }
}