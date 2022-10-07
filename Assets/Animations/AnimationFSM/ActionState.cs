using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
using UnityEngine.Events;

namespace ScavengerWorld
{
    public class ActionState : State
    {
        private AnimancerLayer baseLayer;
        private AnimancerLayer staggerLayer;
        private ClipTransition action;
        private ClipTransition stagger;

        public UnityAction OnAnimationEnd;

        public bool IsPlaying { get; private set; }
        public bool Enable { get; set; }

        public override bool CanEnterState => Enable;

        public override bool CanExitState => !Enable;

        public ActionState(AnimancerLayer baseLayer, AnimancerLayer staggerLayer, ClipTransition stagger)
        {
            this.baseLayer = baseLayer;
            this.staggerLayer = staggerLayer;
            this.stagger = stagger;
            action = new();
            IsPlaying = false;
        }

        public override void OnEnterState()
        {
            if (action.IsLooping)
            {
                IsPlaying = true;
                baseLayer.Play(action);
                return;
            }

            IsPlaying = true;
            var state = baseLayer.Play(action);
            state.Events.OnEnd = OnEnd;
        }

        public void AnimateStagger()
        {
            staggerLayer.Play(stagger);
        }

        public void SetActionAnimation(AnimationClip clip, float speed=1)
        {
            action.Clip = clip;
            action.Speed = speed;
        }

        public void Reset()
        {            
            IsPlaying = false;
            action.Clip = null;
            action.Speed = 1;
        }

        private void OnEnd()
        {
            baseLayer.Stop();
            Reset();
            OnAnimationEnd?.Invoke();
        }
    }
}