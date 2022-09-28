using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class AnimationController : MonoBehaviour
    {
        private const string SPEED = "speed";

        private Mover mover;
        private Animator animator;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetFloat(SPEED, mover.Speed);
        }
    }
}