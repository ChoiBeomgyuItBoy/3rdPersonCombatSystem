using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Play Animation")]
    public class PlayAnimation : StateAction
    {
        [SerializeField] string animationName = "";
        [SerializeField] float transitionDuration = 0.1f;
        [SerializeField] bool inFixedTime = false;

        protected override void OnEnter()
        {
            Animator animator = controller.GetComponent<Animator>();

            if(inFixedTime)
            {
                animator.CrossFadeInFixedTime(animationName, transitionDuration);
            }
            else
            {
                animator.CrossFade(animationName, transitionDuration);
            }
        }

        protected override void OnTick() { }

        protected override void OnExit() { }
    }
}