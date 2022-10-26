using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI;
using Unity.MLAgents.Actuators;
using System;
using Unity.MLAgents.Policies;
using UnityEngine.Events;
using Animancer;
using System.Collections.ObjectModel;

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
        [SerializeField] private float rotateSpeed = 20f;
        [SerializeField] private Sensor sensor;
        [SerializeField] private Inventory inventory;

        [Header("Class & Attributes")]
        [SerializeField] private UnitClass unitClass;
        [SerializeField] private Attributes attributes;

        [Header("Weapons")]
        [SerializeField] private Weapon attackWeapon;
        [SerializeField] private Weapon defenseWeapon;

        [Header("Actions")]
        [SerializeField] private List<Action> attackActions;
        [SerializeField] private List<Action> defendActions;
        [SerializeField] private List<Action> defaultActions;

        [Header("Animations")]
        [SerializeField] private LinearMixerTransition defaultLocomotion;
        [SerializeField] private ClipTransition deathAnimation;

        private MeshRenderer meshRenderer;

        private Stats stats;
        private Interactable interactable;
        private Damageable damageable;
        private AIController aiController;
        private CharacterVitals vitals;
        private CapsuleCollider unitCollider;
        private ActorAgent actorAgent;
        private BehaviorParameters behaviorParameters;

        public UnitClass UnitClass => unitClass;
        public Weapon AttackWeapon => attackWeapon;
        public Weapon DefenseWeapon => defenseWeapon;
        public Attributes Attributes => attributes;
        public Stats Stats => stats;

        public ReadOnlyCollection<Action> AttackActions => attackActions.AsReadOnly();
        public ReadOnlyCollection<Action> DefendActions => defendActions.AsReadOnly();
        public ReadOnlyCollection<Action> DefaultActions => defaultActions.AsReadOnly();
        
        public Unit StorageDepot { get; set; }
        public Interactable Interactable => interactable;
        public Damageable Damageable => damageable;
        public AIController AIController => aiController;
        public CharacterVitals Vitals => vitals;
        
        public ActorAgent ActorAgent => actorAgent;
        public ArenaManager ArenaManager { get; private set; }

        public int TeamId { get; set; }
        public float HowFullIsInventory => inventory.HowFull();
        public int InventoryItemCount => inventory.ItemCount;
        public bool IsStorageDepot => inventory.IsStorageDepot;
        public float SensorRange => sensor.Radius;
        public float RotateSpeed => rotateSpeed;
        public LinearMixerTransition DefaultLocomotion => defaultLocomotion;
        public ClipTransition DeathAnimation => deathAnimation;

        public event UnityAction<float> OnRewardEarned;

        void Awake()
        {
            damageable = GetComponent<Damageable>();
            interactable = GetComponent<Interactable>();
            aiController = GetComponent<AIController>();
            vitals = GetComponent<CharacterVitals>();
            unitCollider = GetComponent<CapsuleCollider>();
            actorAgent = GetComponent<ActorAgent>();
            behaviorParameters = GetComponentInChildren<BehaviorParameters>();
            ArenaManager = GetComponentInParent<TeamGroup>()?.GetComponentInParent<ArenaManager>();

            stats = new Stats(attributes);

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

        public void ApplyDefenseBoost()
        {
            stats.AddDamageReduction(defenseWeapon.DamageReduction); // Hardcoding to use defense weapon for now
        }

        public void UnapplyDefenseBoost()
        {
            stats.ReduceDamageReduction(defenseWeapon.DamageReduction); // Hardcoding to use defense weapon for now
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

        public void Disable()
        {
            unitCollider.enabled = false;
            aiController.Disable();
            attackWeapon?.Disable();
            defenseWeapon?.Disable();
        }
    }
}