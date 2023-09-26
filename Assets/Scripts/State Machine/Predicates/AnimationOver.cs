using UnityEngine;

namespace CombatSystem.StateMachine.Conditions
{
    [CreateAssetMenu(menuName = "State Machine/Predicates/Animation Over")]
    public class AnimationOver : StatePredicate
    {
        [SerializeField] string animationTag = "";
        [SerializeField] [Range(0,0.99f)] float timeToSuccess = 0.99f;
        Animator animator;

        protected override void OnEnter()
        {
            animator = controller.GetComponent<Animator>();
        }

        protected override bool OnCheck()
        {
            return IsFinished();
        }

        protected override void OnExit() { }

        private bool IsFinished()
        {
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);

            if(!animator.IsInTransition(0) && currentInfo.IsTag(animationTag))
            {
                if(currentInfo.normalizedTime >= timeToSuccess)
                {
                    return true;
                }
            }

            return false;
        }
    }
}