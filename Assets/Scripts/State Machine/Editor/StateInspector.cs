using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

namespace CombatSystem.StateMachine.Editor
{
    public class StateInspector : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<StateInspector, UxmlTraits> { }

        State currentState;
        ScrollView actionListScrollView;
        ScrollView transitionListScrollView;
        TextField nameTextField;
        DropdownField actionsDropdown;
        Button addActionButton;

        public StateInspector() 
        { 
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(StateMachineEditor.path + "StateInspector.uxml");
            visualTree.CloneTree(this);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachineEditor.path + "StateInspector.uss");
            styleSheets.Add(styleSheet);

            transitionListScrollView = this.Q<ScrollView>("transitionListScrollView");

            actionListScrollView = this.Q<ScrollView>("actionListScrollView");

            nameTextField = this.Q<TextField>("nameTextField");

            actionsDropdown = this.Q<DropdownField>("actionsDropdown");
            actionsDropdown.choices.Clear();
            actionsDropdown.value = "";

            addActionButton = this.Q<Button>("addActionButton");
            addActionButton.clicked += AddAction;
        }

        public void UpdateSelection(StateItem stateItem)
        {
            currentState = stateItem.GetState();

            SetName();
            SetActionsDropdown();
            SetActions();
            SetTransitions();
        }

        private void SetName()
        {
            nameTextField.value = currentState.stateName;
            nameTextField.RegisterValueChangedCallback(OnNameTextChanged);
        }

        private void SetActionsDropdown()
        {
            var actionTypes = TypeCache.GetTypesDerivedFrom(typeof(StateAction));

            actionsDropdown.choices.Clear();

            foreach(var actionType in actionTypes)
            {
                actionsDropdown.choices.Add(actionType.Name);
            }
        }

        private void AddAction()
        {
            if(actionsDropdown.value == "")
            {
                return;
            }

            string typeName = $"CombatSystem.StateMachine.Actions.{actionsDropdown.value},Assembly-CSharp";
            Type actionType = Type.GetType(typeName, true);

            if(actionType != null)
            {
                currentState.CreateAction(actionType);
            }
        }

        private void SetActions()
        {
            actionListScrollView.Clear();

            List<StateAction> actions = currentState.GetActions();

            if(actions.Count == 0)
            {
                AddToScrollView(actionListScrollView, "List is Empty");
                return;
            }

            actions.ForEach(action =>
            {
                AddToScrollView(actionListScrollView, $"{action.name}");
            });
        }

        private void SetTransitions()
        {
            transitionListScrollView.Clear();

            List<StateTransition> transitions = currentState.GetTransitions();

            if(transitions.Count == 0)
            {
                AddToScrollView(transitionListScrollView, "List is Empty");
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

        private void OnNameTextChanged(ChangeEvent<string> evt)
        {
            currentState.stateName = evt.newValue;
        }
    }
}
