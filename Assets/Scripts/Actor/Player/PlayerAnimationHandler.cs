using Actor.Player;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Actor.Player
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        public Animator animator;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int DoThrowHash = Animator.StringToHash("DoThrow");

        public void PlayMovement(float horizontal, float vertical)
        {
            float moveSpeed = new Vector2(horizontal, vertical).magnitude;

            animator.SetFloat(SpeedHash, moveSpeed);
        }

        public void PlayAttack()
        {
            animator.SetTrigger(DoThrowHash);
        }
    }
}