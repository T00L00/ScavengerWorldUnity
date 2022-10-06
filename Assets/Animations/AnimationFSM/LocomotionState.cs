using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace ScavengerWorld
{
    public class LocomotionState : State
    {
        private AnimancerLayer baseLayer;
        private AnimancerLayer staggerLayer;
        private LinearMixerTransition locomotion;
        private ClipTransition stagger;

        public bool Enable { get; set; }

        public override bool CanEnterState => Enable;
        public override bool CanExitState => !Enable;

        public LocomotionState(AnimancerLayer baseLayer, AnimancerLayer staggerLayer, LinearMixerTransition locomotion, ClipTransition stagger)
        {
            this.baseLayer = baseLayer;
            this.staggerLayer = staggerLayer;
            this.locomotion = locomotion;
            this.stagger = stagger;
        }

        public override void OnEnterState()
        {
            baseLayer.Play(locomotion);
        }

        public void AnimateStagger()
        {
            staggerLayer.Play(stagger);
        }

        public void SetSpeed(float speed)
        {
            locomotion.State.Parameter = speed;
        }
    }
}