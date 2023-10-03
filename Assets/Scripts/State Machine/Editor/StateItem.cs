using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace CombatSystem.StateMachine.Editor
{
    public class StateItem : Node
    {
        State state;
        Port inputPort;
        Port outputPort;

        public StateItem(State state) : base(StateMachineEditor.path + "StateItem.uxml")
        {
            this.state = state;
            title = state.GetName();
            viewDataKey = state.name;
            style.left = state.GetPosition().x;
            style.top = state.GetPosition().y;
            CreatePorts();
        }

        public State GetState()
        {
            return state;
        }

        public StateEdge ConnectTo(StateItem stateItem)
        {
            return outputPort.ConnectTo<StateEdge>(stateItem.inputPort);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Make Transition", a => TransitionSelection());
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            state.SetPosition(new Vector2(newPos.x, newPos.y));
        }

        private class SimulatePress : MouseDownEvent
        {
            public SimulatePress(Vector3 position)
            {
                mousePosition = new Vector2(position.x, position.y);
                button = 0;
            }
        }

        private void TransitionSelection()
        {
            RemoveAt(0);
            Insert(childCount, outputPort);
            outputContainer.SendEvent(new SimulatePress(outputPort.GetGlobalCenter()));
            RemoveAt(childCount - 1);
            Insert(0, outputPort);
        }

        private void CreatePorts()
        {
            inputPort = Port.Create<StateEdge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.AddToClassList("invisiblePort");
            Insert(0, inputPort);

            outputPort = Port.Create<StateEdge>(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.AddToClassList("invisiblePort");
            Insert(0, outputPort);
        }
    }
}