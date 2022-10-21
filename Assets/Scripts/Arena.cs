using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScavengerWorld
{
    public class Arena : MonoBehaviour
    {
        [SerializeField] private GameObject warriorPrefab;
        [SerializeField] private GameObject gathererPrefab;
        [SerializeField] private List<Team> teams;

        private FoodSpawner foodSpawner;

        private void Awake()
        {
            foodSpawner = GetComponent<FoodSpawner>();            
        }

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].SetTeamId(i);
                teams[i].Init(warriorPrefab, gathererPrefab);
            }
            foodSpawner.CreateFood();

            //NavMesh.avoidancePredictionTime = 0.5f;
            //NavMesh.pathfindingIterationsPerFrame = 1000;
        }
    }
}