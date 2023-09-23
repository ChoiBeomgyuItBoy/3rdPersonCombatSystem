using UnityEngine;

namespace CombatSystem.StateMachine.Actions
{
    [CreateAssetMenu(menuName = "State Machine/Actions/Play Animation")]
    public class PlayAnimation : StateAction
    {
        [SerializeField] string animationName = "";
        [SerializeField] float transitionDuration = 0.1f;

        protected override void OnEnter()
        {
            Animator animator = controller.GetComponent<Animator>();
            animator.CrossFade(animationName, transitionDuration);
        }

        protected override void OnTick() { }

        protected override void OnExit() { }
    }
}