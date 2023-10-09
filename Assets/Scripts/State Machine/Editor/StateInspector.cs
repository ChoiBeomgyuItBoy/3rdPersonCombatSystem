using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

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

            Undo.undoRedoPerformed += Refresh;
        }

        public void UpdateSelection(State state)
        {
            if(currentState != null)
            {
                currentState.onChange -= Refresh;
            }

            currentState = state;

            if(currentState != null)
            {
                currentState.onChange += Refresh;
            }

            Refresh();
        }

        private void Refresh()
        {
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
                string formattedType = Regex.Replace(actionType.Name, @"([a-z])([A-Z0-9])", "$1 $2");
                actionsDropdown.choices.Add(formattedType);
            }
        }

        private void SetActions()
        {
            actionListScrollView.Clear();

            List<StateAction> actions = currentState.GetActions();

            if(actions.Count == 0)
            {
                actionListScrollView.Add(GetStyledLabel("List is Empty"));
                return;
            }

            actions.ForEach(action => CreateActionItem(action));
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
                SetActions();
            }
        }

        private void CreateActionItem(StateAction action)
        {
            ActionItem actionItem = new ActionItem(action);
            actionItem.GetRemoveActionButton().clicked += () => RemoveAction(action);
            actionItem.GetEditActionButton().clicked += () => SelectAction(action);
            actionListScrollView.Add(actionItem);
        }

        private void RemoveAction(StateAction action)
        {
            currentState.RemoveAction(action);
        }

        private void SelectAction(StateAction action)
        {
            Selection.activeObject = action;
        }

        private void SetTransitions()
        {
            transitionListScrollView.Clear();

            List<StateTransition> transitions = currentState.GetTransitions();

            if(transitions.Count == 0)
            {
                AddTransitionText("List is Empty");
                return;
            }

            transitions.ForEach(transition =>
            {
                AddTransitionText($"{currentState.stateName} -> {transition.GetTrueState().stateName}");
            });
        }

        private void AddTransitionText(string text)
        {
            Label transitionLabel = GetStyledLabel(text);
            transitionListScrollView.Add(transitionLabel);
        }

        private Label GetStyledLabel(string text)
        {
            Label transitionLabel = new Label(text);

            transitionLabel.style.paddingLeft = new StyleLength(new Length(3, LengthUnit.Percent));
            transitionLabel.style.paddingRight = new StyleLength(new Length(3, LengthUnit.Percent));
            transitionLabel.style.paddingTop = new StyleLength(new Length(1, LengthUnit.Percent));
            transitionLabel.style.paddingBottom = new StyleLength(new Length(1, LengthUnit.Percent));

            return transitionLabel;
        }

        private void OnNameTextChanged(ChangeEvent<string> evt)
        {
            currentState.stateName = evt.newValue;
        }
    }
}
