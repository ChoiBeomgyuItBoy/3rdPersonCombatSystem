using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace CombatSystem.StateMachine.Editor
{
    public class StateMachineEditor : EditorWindow
    {
        StateMachineView stateMachineView;
        StateInspector stateInspector;

        public const string path = "Assets/Scripts/State Machine/Editor/";

        [MenuItem("Window/State Machine Editor")]
        public static void ShowEditorWindow()
        {
            StateMachineEditor window = GetWindow<StateMachineEditor>();
            window.titleContent = new GUIContent("State Machine Editor");
        }

        [OnOpenAsset(1)]
        public static bool OpenBehaviourTree(int instanceID, int line)
        {
            StateMachine stateMachine = EditorUtility.InstanceIDToObject(instanceID) as StateMachine;

            if(stateMachine != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "StateMachineEditor.uxml");
            visualTree.CloneTree(root);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path + "StateMachineEditor.uss");
            root.styleSheets.Add(styleSheet);

            stateMachineView = root.Q<StateMachineView>();
            stateInspector = root.Q<StateInspector>();

            stateMachineView.onStateSelected = OnStateSelectionChange;

            OnSelectionChange();
        }

        private void OnSelectionChange() 
        {
            StateMachine stateMachine = Selection.activeObject as StateMachine;

            if(stateMachine != null)
            {
                stateMachineView.PopulateView(stateMachine);
            }
        }

        private void OnStateSelectionChange(StateItem stateItem)
        {
            stateInspector.UpdateSelection(stateItem.GetState());
        }
    }
}
