using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace ScavengerWorld
{
    public class StaggerState : State
    {
        private AnimancerComponent animancer;
        private ClipTransition animation;

        public UnityAction OnAnimationEnd;

        public bool IsPlaying { get; private set; }
        public bool Enable { get; set; }

        public override bool CanEnterState => Enable;

        public override bool CanExitState => !Enable;

        public StaggerState(AnimancerComponent animancer, ClipTransition animation)
        {
            this.animancer = animancer;
            this.animation = animation;
            IsPlaying = false;
        }

        public override void OnEnterState()
        {
            IsPlaying = true;
            var state = animancer.Play(animation);
            state.Events.OnEnd = OnEnd;
        }
        
        private void OnEnd()
        {
            animancer.Stop();
            Enable = false;
            IsPlaying = false;
            OnAnimationEnd?.Invoke();
        }
    }
}