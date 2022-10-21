using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public static class Utils
    {
        public static string Choice(this System.Random rnd, IEnumerable<string> choices, IEnumerable<float> weights)
        {
            var cumulativeWeight = new List<float>();
            float last = 0;
            foreach (var cur in weights)
            {
                last += cur;
                cumulativeWeight.Add(last);
            }
            int choice = Random.Range(0, 100);
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
    }
}