using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;

namespace ScavengerWorld.AI
{
    public class TeamGroup : MonoBehaviour
    {
        [SerializeField] private Unit storage;
        [SerializeField] private List<Unit> units;

        public int FoodStored => storage.InventoryItemCount;
        public int TeamId { get; set; }

        private Color teamColor;
        private SimpleMultiAgentGroup group = new();

        void Awake()
        {
            foreach (Unit u in units)
            {
                if (u.ActorAgent != null)
                {
                    group.RegisterAgent(u.ActorAgent);
                }
                
                u.StorageDepot = storage;
            }
        }

        public void EndEpisode(float reward)
        {
            group.SetGroupReward(reward);
            group.EndGroupEpisode();
        }

        public void ResetTeam(int teamId)
        {
            // TODO: Randomly place units around the center transform

            foreach (Unit u in units)
            {
                u.TeamId = teamId;
                u.Damageable.ResetHealth();
                u.ActionRunner.CancelCurrentAction();
                u.Mover.ResetNavigator();
                u.transform.position = storage.transform.position;                              
                u.gameObject.SetActive(true);
            }
            storage.TeamId = teamId;
            storage.RemoveAllItems();
            storage.Damageable.ResetHealth();
            storage.gameObject.SetActive(true);
        }

        public void SetTeamColor(Color c)
        {
            teamColor = c;
            storage.SetColor(teamColor);
            foreach (Unit u in units)
            {
                u.SetColor(teamColor);
            }
        }

        public bool IsAlive()
        {
            return units.Any(u => u.gameObject.activeInHierarchy);
        }
    }

}