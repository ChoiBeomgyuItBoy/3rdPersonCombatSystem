using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CombatSystem.StateMachine.Editor
{
    public class ActionItem : VisualElement
    {
        Label actionName;
        Button editActionButton;
        Button removeActionButton;

        public ActionItem(StateAction action)
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(StateMachineEditor.path + "ActionItem.uxml");
            visualTree.CloneTree(this);

            actionName = this.Q<Label>();
            editActionButton = this.Q<Button>("editActionButton");
            removeActionButton = this.Q<Button>("removeActionButton");

            string formattedName = Regex.Replace(action.name, @"([a-z])([A-Z0-9])", "$1 $2");
            action.description = formattedName;
            actionName.bindingPath = "description";
            actionName.Bind(new SerializedObject(action));
        }

        public Button GetEditActionButton()
        {
            return editActionButton;
        }

        public Button GetRemoveActionButton()
        {
            return removeActionButton;
        }
    }
}