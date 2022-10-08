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
        [SerializeField] private LinearMixerTransition locomotion;
        [SerializeField] private ClipTransition death;

        [SerializeField] private ClipTransition stagger;
        [SerializeField] private AvatarMask staggerMask;

        private AnimancerLayer baseLayer;
        private AnimancerLayer staggerLayer;

        private Unit unit;
        private Mover mover;
        private LocomotionState locomotionState;
        private ActionState actionState;
        private DeathState deathState;
        private StateMachine<State>.WithDefault stateMachine;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            mover = GetComponent<Mover>();

            baseLayer = animancer.Layers[0];
            staggerLayer = animancer.Layers[1];
            staggerLayer.SetMask(staggerMask);
            staggerLayer.SetDebugName("Stagger Layer");
            stagger.Events.OnEnd = OnStaggerEnd;

            locomotionState = new(baseLayer, staggerLayer, locomotion, stagger);
            actionState = new(baseLayer, staggerLayer, stagger);
            deathState = new(animancer, death);

            stateMachine = new StateMachine<State>.WithDefault(locomotionState);            
        }

        

        // Start is called before the first frame update
        void Start()
        {
            actionState.OnAnimationEnd += OnAnimationEnd;
        }

        private void OnDestroy()
        {
            actionState.OnAnimationEnd -= OnAnimationEnd;
        }

        // Update is called once per frame
        void Update()
        {
            if (stateMachine.CurrentState == locomotionState)
            {
                locomotionState.SetSpeed(mover.NavigatorSpeed);
            }
        }

        public void AnimateStagger()
        {
            unit.Mover.DisableMovement();            
            if (stateMachine.CurrentState == locomotionState)
            {
                locomotionState.AnimateStagger();
            }

            if (stateMachine.CurrentState == actionState)
            {                
                actionState.AnimateStagger();
            }
        }

        private void OnStaggerEnd()
        {
            staggerLayer.StartFade(0, AnimancerPlayable.DefaultFadeDuration);
            unit.ActionRunner.CancelCurrentAction();
            unit.Mover.EnableMovement();
        }

        public void AnimateDeath()
        {
            locomotionState.Enable = false;
            actionState.Enable = false;
            deathState.Enable = true;
            stateMachine.TrySetState(deathState);
        }

        public void AnimateAttackAction(AnimationClip clip)
        {
            locomotionState.Enable = false;
            deathState.Enable = false;
            actionState.Enable = true;
            actionState.SetActionAnimation(clip, unit.Stats.attackSpeed);
            stateMachine.TrySetState(actionState);
            unit.Mover.DisableMovement();
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
            locomotionState.Enable = true;
            stateMachine.TrySetDefaultState();
            actionState.Reset();            
        }

        public bool ActionIsPlaying => actionState.IsPlaying;

        private void OnAnimationEnd()
        {
            deathState.Enable = false;
            actionState.Enable = false;
            locomotionState.Enable = true;
            stateMachine.TrySetDefaultState();
            unit.Mover.EnableMovement();
        }

        public void HitEvent()
        {
            //RaycastHit[] hits = Physics.SphereCastAll(new Ray(transform.position, transform.forward), 1f, 0.5f);
            //foreach (RaycastHit h in hits)
            //{
            //    Unit enemyUnit = h.collider.GetComponent<Unit>();
            //    if (enemyUnit != null && enemyUnit != unit)
            //    {
            //        Vector3 vectorToEnemy = (enemyUnit.transform.position - transform.position).normalized;
            //        if (Vector3.Dot(vectorToEnemy, transform.forward) > 0.5) // hit occured within 90 degree arc in front
            //        {
            //            float totalDamage = unit.Weapon.DamageModifier * unit.Stats.baseDamage;
            //            enemyUnit.Damageable.TakeDamage(totalDamage);
            //            enemyUnit.Attributes.Poise.Reduce(totalDamage * 0.5f);
            //            enemyUnit.Attributes.Energy.Reduce(totalDamage * 0.2f);
            //            //Debug.Log("hit!");
            //        }
            //    }
            //}
        }
    }
}