using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace ScavengerWorld
{
    public class LocomotionState : State
    {
        private AnimancerComponent animancer;
        private LinearMixerTransition animation;

        public bool Enable { get; set; }

        public override bool CanEnterState => Enable;
        public override bool CanExitState => !Enable;

        public LocomotionState(AnimancerComponent animancer, LinearMixerTransition animation)
        {
            this.animancer = animancer;
            this.animation = animation;
        }

        public override void OnEnterState()
        {
            animancer.Play(animation);
        }

        public void SetSpeed(float speed)
        {
            animation.State.Parameter = speed;
        }
    }
}