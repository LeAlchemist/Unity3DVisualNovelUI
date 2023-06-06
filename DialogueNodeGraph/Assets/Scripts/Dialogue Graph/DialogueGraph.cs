using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class DialogueGraph : ScriptableObject
{
    public Node rootNode;
    public Node.State graphState = Node.State.Running;
    public List<Node> nodes = new List<Node>();

    public Node.State Update()
    {
        if (rootNode.state == Node.State.Running)
        {
            graphState = rootNode.Update();
        }

        return graphState;
    }

    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        Node node = parent as Node;
        if (node)
        {
            node.children.Add(child);
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        Node node = parent as Node;
        if (node)
        {
            node.children.Remove(child);
        }
    }

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        Node node = parent as Node;
        if (node)
        {
            return node.children;
        }

        return children;
    }
}
