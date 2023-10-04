using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using System;

namespace CombatSystem.StateMachine.Editor
{
    public class StateItem : Node
    {
        State state;
        Port inputPort;
        Port outputPort;
        Label titleLabel;
        public Action<StateItem> onStateSelected;

        public StateItem(State state) : base(StateMachineEditor.path + "StateItem.uxml")
        {
            this.state = state;
            title = state.stateName;
            viewDataKey = state.name;
            style.left = state.GetPosition().x;
            style.top = state.GetPosition().y;
            BindTitle(state);
            CreatePorts();
        }

        private void BindTitle(State state)
        {
            titleLabel = this.Q<Label>("title-label");
            titleLabel.bindingPath = "stateName";
            titleLabel.Bind(new SerializedObject(state));
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
            Undo.RecordObject(state,"(State Machine) Set Position");
            state.SetPosition(new Vector2(newPos.x, newPos.y));
            EditorUtility.SetDirty(state);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            Selection.activeObject = state;
            onStateSelected?.Invoke(this);
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