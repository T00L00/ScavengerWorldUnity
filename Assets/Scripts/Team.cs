using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [Serializable]
    public class Team
    {
        [SerializeField] private int warriorsCount;
        [SerializeField] private int gatherersCount;
        [SerializeField] private Unit storage;

        private int teamId;
        private readonly List<Unit> units = new();

        public int TeamId => teamId;

        public void Init(GameObject warriorPrefab, GameObject gathererPrefab)
        {
            for (int i = 0; i < warriorsCount; i++)
            {
                units.Add(GameObject.Instantiate(warriorPrefab, storage.transform.position, Quaternion.identity).GetComponent<Unit>());
            }

            for (int i = 0; i < gatherersCount; i++) 
            {
                units.Add(GameObject.Instantiate(gathererPrefab, storage.transform.position, Quaternion.identity).GetComponent<Unit>());
            }

            storage.TeamId = teamId;
            foreach (Unit unit in units)
            {
                unit.TeamId = teamId;
            }
        }

        public void SetTeamId(int id)
        {
            teamId = id;
        }

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
        }        
    }
}