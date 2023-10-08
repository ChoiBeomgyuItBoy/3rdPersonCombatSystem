using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/New State")]
    public class State : ScriptableObject
    {
        public string stateName = "New State";
        [SerializeField] Vector2 position = Vector2.zero;
        [SerializeField] List<StateAction> actions = new List<StateAction>();
        [SerializeField] List<StateTransition> transitions = new List<StateTransition>();
        StateController controller;
        public event Action onChange;
        public event Action onExit;

        public Vector2 GetPosition()
        {
            return position;
        }

        public List<StateAction> GetActions()
        {
            return actions;
        }

        public List<StateTransition> GetTransitions()
        {
            return transitions;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "State Moved");
            this.position = position;
            EditorUtility.SetDirty(this);
        }

        public StateTransition CreateTransition(State endState)
        {
            if(HasTransitionTo(endState))
            {
                return null;
            }
            
            StateTransition transition = new StateTransition();
            Undo.RecordObject(this, "Transition Added");

            transition.SetTrueState(endState);
            transitions.Add(transition);

            EditorUtility.SetDirty(this);
            onChange?.Invoke();

            return transition;
        }

        public void RemoveTransition(State endState)
        {
            StateTransition transition = GetTransition(endState);

            if(transition != null)
            {
                Undo.RecordObject(this, "Transition Removed");
                transitions.Remove(transition);
                EditorUtility.SetDirty(this);
                onChange?.Invoke();
            }
        }

        public StateAction CreateAction(Type type)
        {   
            StateAction action = CreateInstance(type) as StateAction;
            action.name = type.Name;

            Undo.RegisterCreatedObjectUndo(action, "Action Created");
            Undo.RecordObject(this, "Action Added");

            actions.Add(action);

            AssetDatabase.AddObjectToAsset(action, this);
            EditorUtility.SetDirty(this);
            onChange?.Invoke();

            return action;
        }

        public void RemoveAction(StateAction action)
        {
            Undo.RecordObject(this, "Action Removed");
            actions.Remove(action);
            EditorUtility.SetDirty(this);
            onChange?.Invoke();
        }
#endif

        public void Tick()
        {
            DoActions();
            CheckConditions();
        }

        public void Exit()
        {
            onExit?.Invoke();
        }

        public void Bind(StateController controller)
        {
            this.controller = controller;
            actions.ForEach((action) => action.Bind(controller, this));
            transitions.ForEach((transition) => transition.Bind(controller, this));
        }

        public State Clone()
        {
            State clone = Instantiate(this);
            clone.actions = actions.ConvertAll((action) => action.Clone());
            clone.transitions = transitions.ConvertAll((transition) => transition.Clone());
            return clone;
        }

        private void DoActions()
        {
            actions.ForEach((action) =>
            {
                action.Tick();
            });
        } 

        private void CheckConditions()
        {
            transitions.ForEach((transition) =>
            {
                if(transition.Check())
                {
                    controller.SwitchState(transition.GetTrueState().name);
                    return;
                }
            });
        }

        private StateTransition GetTransition(State trueState)
        {
            return transitions.SingleOrDefault(transition => transition.GetTrueState() == trueState);
        }

        private bool HasTransitionTo(State trueState)
        {
            return GetTransition(trueState) != null;
        }
    }
}