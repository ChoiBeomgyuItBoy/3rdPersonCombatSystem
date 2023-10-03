using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CombatSystem.StateMachine.Editor
{
    public class StateMachineView : GraphView
    {
        StateMachine stateMachine;

        public new class UxmlFactory : UxmlFactory<StateMachineView, UxmlTraits> { }

        public StateMachineView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            StyleSheet uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachineEditor.path + "StateMachineEditor.uss");
            styleSheets.Add(uss);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        public void PopulateView(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            stateMachine.GetStates().ForEach(state => CreateStateItem(state));
            stateMachine.GetStates().ForEach(state => CreateTransitions(state));
        }

        private void OnUndoRedo()
        {
            PopulateView(stateMachine);
            AssetDatabase.SaveAssets();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            List<Edge> edgesToCreate = graphViewChange.edgesToCreate;

            if(edgesToCreate != null)
            {
                for(int i = edgesToCreate.Count - 1; i >= 0; i--)
                {
                    Edge edge = edgesToCreate[i];
                    StateEdge stateEdge = edge as StateEdge;
                    StateItem startItem = edge.output.node as StateItem;
                    StateItem endItem = edge.input.node as StateItem;

                    if(stateMachine.CreateTransition(startItem.GetState(), endItem.GetState()) == null)
                    {
                        edgesToCreate.RemoveAt(i);
                    }   
                }
            }

            List<GraphElement> elementsToRemove = graphViewChange.elementsToRemove;

            if(elementsToRemove != null)
            {
                for (int i = elementsToRemove.Count - 1; i >= 0 ; i--)
                {
                    StateItem stateItem = elementsToRemove[i] as StateItem;

                    if(stateItem != null)
                    {
                        stateItem.Query<Port>().ForEach(port => elementsToRemove.AddRange(port.connections));
                        stateMachine.RemoveTransitionsWithState(stateItem.GetState());
                        stateMachine.RemoveState(stateItem.GetState());
                    }
                    
                    StateEdge stateEdge = elementsToRemove[i] as StateEdge;

                    if(stateEdge != null)
                    {
                        StateItem startItem = stateEdge.output.node as StateItem;
                        StateItem endItem = stateEdge.input.node as StateItem;
                        stateMachine.RemoveTransition(startItem.GetState(), endItem.GetState());
                    }
                }
            }

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            Vector2 mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
            evt.menu.AppendAction("Add State", action => CreateState(mousePosition));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => 
                endPort.direction != startPort.direction && 
                endPort.node != startPort.node).ToList();
        }

        private void CreateState(Vector2 position)
        {
            State state = stateMachine.CreateState();
            CreateStateItem(state);
            GetStateItem(state).SetPosition(new Rect(position.x, position.y, 0, 0));
        }

        private void CreateStateItem(State state)
        {
            StateItem stateItem = new StateItem(state);
            AddElement(stateItem);
        }

        private StateItem GetStateItem(State state)
        {
            return GetNodeByGuid(state.name) as StateItem;
        }

        private void CreateTransitions(State state)
        {   
            StateItem stateItem = GetStateItem(state); 

            state.GetTransitions().ForEach(transition =>
            {
                StateItem trueStateItem = GetStateItem(transition.GetTrueState());
                StateEdge edge = stateItem.ConnectTo(trueStateItem);
                AddElement(edge);
            });
        }
    }
}
