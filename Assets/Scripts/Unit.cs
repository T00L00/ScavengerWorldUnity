using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI;
using Unity.MLAgents.Actuators;
using System;
using Unity.MLAgents.Policies;
using UnityEngine.Events;

namespace ScavengerWorld
{
    /// <summary>
    /// Responsible for holding gameplay data about unit and 
    /// handling unit-specific changes.
    /// (e.g. taking damage, inventory changes, stats)
    /// </summary>
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(Interactable))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Sensor sensor;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Stats stats;

        private MeshRenderer meshRenderer;
        private Interactable interactable;
        private Damageable damageable;
        private Mover mover;
        private ActionRunner actionRunner;
        private ActorAgent actorAgent;
        private BehaviorParameters behaviorParameters;
        
        public Stats Stats => stats;
        public Unit StorageDepot { get; set; }
        public Interactable Interactable => interactable;
        public Damageable Damageable => damageable;
        public Mover Mover => mover;
        public ActionRunner ActionRunner => actionRunner;
        public ActorAgent ActorAgent => actorAgent;
        public ArenaManager ArenaManager { get; private set; }

        public int TeamId { get; set; }
        public float HowFullIsInventory => inventory.HowFull();
        public int InventoryItemCount => inventory.ItemCount;
        public bool IsStorageDepot => inventory.IsStorageDepot;
        public float SensorRange => sensor.Radius;

        public event UnityAction<float> OnRewardEarned;

        void Awake()
        {
            damageable = GetComponent<Damageable>();
            interactable = GetComponent<Interactable>();
            mover = GetComponent<Mover>();
            actionRunner = GetComponent<ActionRunner>();
            actorAgent = GetComponent<ActorAgent>();
            behaviorParameters = GetComponentInChildren<BehaviorParameters>();
            ArenaManager = GetComponentInParent<TeamGroup>().GetComponentInParent<ArenaManager>();

            meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer is null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (behaviorParameters != null) 
                behaviorParameters.TeamId = TeamId;
        }

        public List<Interactable> Pulse()
        {
            return sensor.Pulse(transform.position);
        }

        public void Attack(Damageable enemy)
        {
            // play little animation
            enemy.TakeDamage(stats.attackDamage);
        }
        
        public void AddItem(Gatherable gatherable)
        {
            if (inventory.HowFull() == 1f) return;

            if (inventory.WillBeOverfilled(stats.gatherRate)) return;

            if (gatherable.AmountAvailable > stats.gatherRate)
            {
                inventory.Add(gatherable.Take(stats.gatherRate));
                return;
            }

            inventory.Add(gatherable.TakeAll());
        }

        public void AddItem(int amount)
        {
            if (inventory.HowFull() == 1f) return;

            if (inventory.WillBeOverfilled(amount)) return;

            inventory.Add(amount);
        }

        public void RemoveItem(int amount)
        {
            inventory.Remove(amount);
        }

        public int RemoveAllItems()
        {
            return inventory.RemoveAll();
        }

        public void SetColor(Color color)
        {
            meshRenderer.material.color = color;
        }

        public void SetReward(float reward)
        {
            OnRewardEarned?.Invoke(reward);
        }
    }
}