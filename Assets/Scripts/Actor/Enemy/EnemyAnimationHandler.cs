using System;
using Unity.Netcode.Components;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyAnimationHandler : NetworkAnimator
    {
        public Animator animator;

        private static readonly int IsDeadHash = Animator.StringToHash("isDead");

        public event Action OnDeathAnimationFinished;

        public void PlayDead()
        {
            animator.SetBool(IsDeadHash, true);
        }

        public void OnDeathAnimEnd()
        {
            OnDeathAnimationFinished?.Invoke();
        }
    }
}