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
    public enum UnitClass
    {
        Gatherer,
        Warrior
    }

    /// <summary>
    /// Responsible for holding gameplay data about unit and 
    /// handling unit-specific changes.
    /// (e.g. taking damage, inventory changes, stats)
    /// </summary>
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(Interactable))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitClass unitClass;
        [SerializeField] private Weapon weapon;
        [SerializeField] private Sensor sensor;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Stats stats;

        private MeshRenderer meshRenderer;
        private Interactable interactable;
        private Damageable damageable;
        private Mover mover;
        private AIController aiController;
        private ActionRunner actionRunner;
        private AnimationController animController;
        private CharacterAttributes attributes;

        private ActorAgent actorAgent;
        private BehaviorParameters behaviorParameters;

        public UnitClass UnitClass => unitClass;
        public Weapon Weapon => weapon;
        public Stats Stats => stats;
        public Unit StorageDepot { get; set; }
        public Interactable Interactable => interactable;
        public Damageable Damageable => damageable;
        public Mover Mover => mover;
        public AIController AIController => aiController;
        public ActionRunner ActionRunner => actionRunner;
        public AnimationController AnimController => animController;
        public CharacterAttributes Attributes => attributes;
        
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
            aiController = GetComponent<AIController>();
            animController = GetComponent<AnimationController>();
            attributes = GetComponent<CharacterAttributes>();
            actorAgent = GetComponent<ActorAgent>();
            behaviorParameters = GetComponentInChildren<BehaviorParameters>();
            ArenaManager = GetComponentInParent<TeamGroup>()?.GetComponentInParent<ArenaManager>();

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
            enemy.TakeDamage(stats.baseDamage);
        }

        public void AddItem(Gatherable gatherable, int amount)
        {
            if (inventory.HowFull() == 1f) return;

            if (inventory.WillBeOverfilled(amount)) return;

            if (gatherable.AmountAvailable > amount)
            {
                inventory.Add(gatherable.Take(amount));
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