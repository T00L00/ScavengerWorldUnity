using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace ScavengerWorld
{
    public class DeathState : State
    {
        private AnimancerComponent animancer;
        private ClipTransition animation;

        public bool Enable { get; set; }

        public override bool CanEnterState => Enable;

        public override bool CanExitState => !Enable;

        public DeathState(AnimancerComponent animancer, ClipTransition transition)
        {
            this.animancer = animancer;
            this.animation = transition;
        }

        public override void OnEnterState()
        {
            animancer.Play(animation);
        }
    }
}

