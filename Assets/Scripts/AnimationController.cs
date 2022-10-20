using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using ScavengerWorld.AI;

namespace ScavengerWorld
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private ClipTransition stagger;
        [SerializeField] private AvatarMask staggerMask;

        private AIController aiController;

        private ClipTransition actionAnimation;
        private AnimancerLayer baseLayer;
        private AnimancerLayer staggerLayer;

        private void Awake()
        {
            aiController = GetComponent<AIController>();

            actionAnimation = new();
            baseLayer = animancer.Layers[0];
            staggerLayer = animancer.Layers[1];
            staggerLayer.SetMask(staggerMask);
            staggerLayer.SetDebugName("Stagger Layer");
            stagger.Events.OnEnd = OnStaggerEnd;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AnimateLocomotion(LinearMixerTransition locomotion)
        {
            baseLayer.Play(locomotion);
        }

        public void AnimateAction(AnimationClip clip, float speed=1)
        {
            //baseLayer.Stop();
            actionAnimation.Clip = clip;
            actionAnimation.Speed = speed;

            if (actionAnimation.IsLooping)
            {
                baseLayer.Play(actionAnimation);
                return;
            }

            var state = baseLayer.Play(actionAnimation);
            state.Events.OnEnd = OnAnimationEnd;
        }

        public void StopActionAnimation()
        {
            baseLayer.Stop();
        }

        public void AnimateStagger()
        {
            staggerLayer.Play(stagger);
        }

        public void AnimateDeath(ClipTransition animation)
        {
            animancer.Play(animation);
        }

        private void OnAnimationEnd()
        {
            baseLayer.Stop();
            actionAnimation.Clip = null;
            actionAnimation.Speed = 1;
            aiController.CancelSelectedAction();
        }

        private void OnStaggerEnd()
        {
            staggerLayer.StartFade(0, AnimancerPlayable.DefaultFadeDuration);
            aiController.CancelSelectedAction();
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