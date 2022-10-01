using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace ScavengerWorld
{
    public class ActionState : State
    {
        private AnimancerComponent animancer;
        private ClipTransition animation;

        public bool IsPlaying { get; private set; }
        public bool Enable { get; set; }

        public override bool CanEnterState => Enable;

        public override bool CanExitState => !Enable;

        public ActionState(AnimancerComponent animancer)
        {
            this.animancer = animancer;
            this.animation = new();
            IsPlaying = false;
        }

        public override void OnEnterState()
        {
            if (animation.IsLooping)
            {
                IsPlaying = true;
                animancer.Play(animation);
                return;
            }

            IsPlaying = true;
            var state = animancer.Play(animation);
            state.Events.OnEnd = OnAnimationEnd;
        }

        public void SetActionAnimation(AnimationClip clip)
        {
            animation.Clip = clip;
        }

        public void Reset()
        {            
            IsPlaying = false;
            animation.Clip = null;
        }

        private void OnAnimationEnd()
        {
            animancer.Stop();
            IsPlaying = false;            
        }
    }
}