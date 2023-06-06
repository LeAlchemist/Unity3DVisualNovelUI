using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DialogueGraphEditor : EditorWindow
{
    DialogueGraphView graphView;
    InspectorView inspectorView;

    [MenuItem("/DialogueGraphEditor/Editor ...")]
    public static void OpenWindow()
    {
        DialogueGraphEditor wnd = GetWindow<DialogueGraphEditor>();
        wnd.titleContent = new GUIContent("DialogueGraphEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DialogueGraphEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogueGraphEditor.uss");
        root.styleSheets.Add(styleSheet);

        graphView = root.Q<DialogueGraphView>();
        inspectorView = root.Q<InspectorView>();
        graphView.OnNodeSelected = OnNodeSelectionChanged;

        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        DialogueGraph graph = Selection.activeObject as DialogueGraph;
        if (graph)
        {
            graphView.PopulateView(graph);
        }
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}