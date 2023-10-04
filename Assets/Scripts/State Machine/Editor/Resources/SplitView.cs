using UnityEngine.UIElements;

namespace CombatSystem.StateMachine.Editor
{
    public class SplitView : TwoPaneSplitView
    {   
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }
    }
}