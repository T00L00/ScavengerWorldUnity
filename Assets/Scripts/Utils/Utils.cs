using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

namespace ScavengerWorld
{
    public static class Utils
    {
        public static string Sample(this System.Random rnd, IEnumerable<string> choices, IEnumerable<float> weights)
        {
            var cumulativeWeight = new List<float>();
            float last = 0;
            foreach (var cur in weights)
            {
                last += cur;
                cumulativeWeight.Add(last);
            }
            float choice = Random.Range(0, last);
            int i = 0;
            foreach (var cur in choices)
            {
                if (choice < cumulativeWeight[i])
                {
                    return cur;
                }
                i++;
            }
            return null;
        }

        public static Vector3 GetRandomPosition(Vector3 sourcePos, float maxDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxDistance + sourcePos;
            Vector3 finalPosition = sourcePos;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }

        public static Vector3 GetRandomPosition(Unit unit, Vector3 sourcePos, float maxDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxDistance + sourcePos;
            Vector3 finalPosition = unit.transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }
    }
}