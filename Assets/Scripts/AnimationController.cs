using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
using System;

namespace ScavengerWorld
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private LinearMixerTransition freeLocomotion;
        [SerializeField] private LinearMixerTransition loadedLocomotion;
        [SerializeField] private ClipTransition death;

        [SerializeField] private ClipTransition stagger;
        [SerializeField] private AvatarMask staggerMask;

        private AnimancerLayer baseLayer;
        private AnimancerLayer staggerLayer;

        private Unit unit;
        private Mover mover;
        private LocomotionState locomotionState;
        private LocomotionState freeLocomotionState;
        private LocomotionState loadedLocomotionState;
        private ActionState actionState;
        private DeathState deathState;
        private StateMachine<State>.WithDefault stateMachine;

        private void Awake()
        {
            unit = GetComponentInParent<Unit>();
            mover = GetComponentInParent<Mover>();

            baseLayer = animancer.Layers[0];
            staggerLayer = animancer.Layers[1];
            staggerLayer.SetMask(staggerMask);
            staggerLayer.SetDebugName("Stagger Layer");
            stagger.Events.OnEnd = OnStaggerEnd;

            freeLocomotionState = new(baseLayer, staggerLayer, freeLocomotion, stagger);
            loadedLocomotionState = new(baseLayer, staggerLayer, loadedLocomotion, stagger);
            actionState = new(baseLayer, staggerLayer, stagger);
            deathState = new(animancer, death);

            stateMachine = new StateMachine<State>.WithDefault(freeLocomotionState);            
        }

        

        // Start is called before the first frame update
        void Start()
        {
            actionState.OnAnimationEnd += OnAnimationEnd;
            locomotionState = unit.HowFullIsInventory <= 0.5f ? freeLocomotionState : loadedLocomotionState;
        }

        private void OnDestroy()
        {
            actionState.OnAnimationEnd -= OnAnimationEnd;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateLocomotionState();
            if (stateMachine.CurrentState == locomotionState)
            {
                locomotionState.SetSpeed(mover.Speed);
            }
        }

        public void AnimateStagger()
        {
            if (stateMachine.CurrentState is LocomotionState)
            {
                locomotionState.AnimateStagger();
            }

            if (stateMachine.CurrentState is ActionState)
            {
                actionState.AnimateStagger();
            }
        }

        private void OnStaggerEnd()
        {
            staggerLayer.StartFade(0, AnimancerPlayable.DefaultFadeDuration);
        }

        public void AnimateDeath()
        {
            locomotionState.Enable = false;
            actionState.Enable = false;
            deathState.Enable = true;
            stateMachine.TrySetState(deathState);
        }

        public void AnimateAction(AnimationClip clip)
        {
            locomotionState.Enable = false;
            deathState.Enable = false;
            actionState.Enable = true;
            actionState.SetActionAnimation(clip);
            stateMachine.TrySetState(actionState);
        }

        public void StopActionAnimation()
        {
            deathState.Enable = false;
            actionState.Enable = false;            
            UpdateLocomotionState();
            locomotionState.Enable = true;
            stateMachine.TrySetState(locomotionState);
            actionState.Reset();
        }

        public void UpdateLocomotionState()
        {
            locomotionState = unit.HowFullIsInventory <= 0.5f ? freeLocomotionState : loadedLocomotionState;            
        }

        public bool ActionIsPlaying => actionState.IsPlaying;

        private void OnAnimationEnd()
        {
            deathState.Enable = false;
            actionState.Enable = false;
            UpdateLocomotionState();
            locomotionState.Enable = true;
            stateMachine.TrySetDefaultState();
        }

        public void HitEvent()
        {
            RaycastHit[] hits = Physics.SphereCastAll(new Ray(transform.position, transform.forward), 2f);
            foreach (RaycastHit h in hits)
            {
                Unit enemyUnit = h.collider.GetComponent<Unit>();
                if (enemyUnit != null && enemyUnit != unit)
                {
                    Vector3 vectorToEnemy = (enemyUnit.transform.position - transform.position).normalized;
                    if (Vector3.Dot(vectorToEnemy, transform.forward) > 0.5) // hit occured within 90 degree arc in front
                    {
                        enemyUnit.Damageable.TakeDamage(unit.Weapon.DamageModifier * unit.Stats.attackDamage);
                        //Debug.Log("hit!");
                    }
                }
            }            
        }
    }
}