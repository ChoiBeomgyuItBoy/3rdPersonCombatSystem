using UnityEditor;
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

            actionName.text = action.name;
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