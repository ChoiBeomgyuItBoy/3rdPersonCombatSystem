using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace CombatSystem.StateMachine.Editor
{
    public class StateInspector : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<StateInspector, UxmlTraits> { }

        State currentState;
        ScrollView actionListScrollView;
        ScrollView transitionListScrollView;

        public StateInspector() 
        { 
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(StateMachineEditor.path + "StateInspector.uxml");
            visualTree.CloneTree(this);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachineEditor.path + "StateInspector.uss");
            styleSheets.Add(styleSheet);

            transitionListScrollView = this.Q<ScrollView>("transitionListScrollView");
            actionListScrollView = this.Q<ScrollView>("actionListScrollView");
        }

        public void UpdateSelection(StateItem stateItem)
        {
            currentState = stateItem.GetState();

            DisplayActions();
            DisplayTransitions();
        }

        private void DisplayActions()
        {
            actionListScrollView.Clear();

            List<StateAction> actions = currentState.GetActions();

            if(actions.Count == 0)
            {
                AddToScrollView(actionListScrollView, "No actions");
                return;
            }

            actions.ForEach(action =>
            {
                AddToScrollView(actionListScrollView, $"{action.name}");
            });
        }

        private void DisplayTransitions()
        {
            transitionListScrollView.Clear();

            List<StateTransition> transitions = currentState.GetTransitions();

            if(transitions.Count == 0)
            {
                AddToScrollView(transitionListScrollView, "No transitions");
                return;
            }

            transitions.ForEach(transition =>
            {
                AddToScrollView(transitionListScrollView, $"{currentState.stateName} -> {transition.GetTrueState().stateName}");
            });
        }

        private void AddToScrollView(ScrollView scrollView, string text)
        {
            Label transitionLabel = new Label(text);
            scrollView.Add(transitionLabel);
        }
    }
}
