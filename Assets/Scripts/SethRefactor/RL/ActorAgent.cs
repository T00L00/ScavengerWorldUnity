using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    public class ActorAgent : Agent
    {
        [SerializeField] private Mover mover;
        [SerializeField] private Unit unit;

        public UnityAction OnNewEpisode;
        public UnityAction<ActionRequest> OnReceivedActions;

        private EnvironmentParameters ResetParams;

        public override void Initialize()
        {
            mover = GetComponent<Mover>();
            unit = GetComponent<Unit>();
            ResetParams = Academy.Instance.EnvironmentParameters;
        }

        public override void OnEpisodeBegin()
        {
            OnNewEpisode?.Invoke();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            var health = unit.Damageable.HealthPercentage;
            var foodInventory = unit.HowFullIsInventory;
            var foodNearby = mover.FoodIsNearby(out _);
            var enemyUnitsNearby = mover.EnemyUnitIsNearby(out _);
            var enemyStorageNearby = mover.EnemyStorageIsNearby(out _);

            sensor.AddObservation(health);
            sensor.AddObservation(foodInventory);
            sensor.AddObservation(foodNearby);
            sensor.AddObservation(enemyUnitsNearby);
            sensor.AddObservation(enemyStorageNearby);
        }


        public override void OnActionReceived(ActionBuffers actions)
        {
            ActionSegment<int> discrete = actions.DiscreteActions;
            var gather = discrete[0];
            var drop = discrete[1];
            var attack = discrete[2];
            var move = discrete[3];
            
            var request = new ActionRequest(ActionType.none);
            if (gather > 0)
            {
                request = new ActionRequest(ActionType.gather);
            }
            else if (drop > 0)
            {
                request = new ActionRequest(ActionType.dropoff);
            }
            else if (attack == 1)
            {
                request = new ActionRequest(ActionType.attackenemy);
            } 
            else if (attack == 2)
            {
                request = new ActionRequest(ActionType.attackstorage);
            }
            else if (move > 0)
            {
                var newPosition = FindMoveToPoint(move);
                request = new ActionRequest(ActionType.move, newPosition);
            }
            OnReceivedActions?.Invoke(request);
        }

        private Vector3 FindMoveToPoint(int move)
        {
            Vector3 position = unit.transform.position;
            Vector3 change = Vector3.zero;
            switch (move)
            {
                case 1: 
                    change = Vector3.forward;
                    break;
                case 2: 
                    change = Vector3.forward + Vector3.right;
                    break;
                case 3: 
                    change = Vector3.right;
                    break;
                case 4:
                    change = Vector3.right + Vector3.back;
                    break;
                case 5: 
                    change = Vector3.back;
                    break;
                case 6: 
                    change = Vector3.back + Vector3.left;
                    break;
                case 7: 
                    change = Vector3.left;
                    break;
                case 8: 
                    change = Vector3.left + Vector3.forward;
                    break;
            }
            return position + change;
        }

        protected void AddReward(string name, float defaultValue)
        {
            var reward = FindReward(name, defaultValue);
            AddReward(reward);
        }

        protected float FindReward(string name, float defaultValue)
        {
            return ResetParams.GetWithDefault(name, defaultValue);
        }
    }
}