using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace ScavengerWorld
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private LinearMixerTransition freeLocomotionAnimation;
        [SerializeField] private LinearMixerTransition loadedLocomotionAnimation;

        private Unit unit;
        private Mover mover;
        private LocomotionState locomotionState;
        private LocomotionState freeLocomotionState;
        private LocomotionState loadedLocomotionState;
        private ActionState actionState;
        private StateMachine<State>.WithDefault stateMachine;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            mover = GetComponent<Mover>();
            freeLocomotionState = new(animancer, freeLocomotionAnimation);
            loadedLocomotionState = new(animancer, loadedLocomotionAnimation);
            actionState = new(animancer);

            stateMachine = new StateMachine<State>.WithDefault(freeLocomotionState);
        }

        // Start is called before the first frame update
        void Start()
        {
            locomotionState = unit.HowFullIsInventory <= 0.5f ? freeLocomotionState : loadedLocomotionState;
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

        public void AnimateAction(AnimationClip clip)
        {
            locomotionState.Enable = false;
            actionState.Enable = true;
            actionState.SetActionAnimation(clip);
            stateMachine.TrySetState(actionState);
        }

        public void StopActionAnimation()
        {
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

        public bool IsPlaying => actionState.IsPlaying;
    }
}