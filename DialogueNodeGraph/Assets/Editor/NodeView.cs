using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public Node node;
    public Port input;
    public Port output;
    public NodeView(Node node) 
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateNodes();
    }

    private Port GeneratePort(Node node, Direction portDirection, Port.Capacity capacity=Port.Capacity.Single)
    {
        return this.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private void CreateNodes()
    {
        switch (node)
        {
            case RootNode:
                //Input Port Data

                //Output Port Data
                output = GeneratePort(node, Direction.Output, Port.Capacity.Single);
                output.portColor = Color.white;
                break;
            case DebugLogNode:
                //Input Port Data
                input = GeneratePort(node, Direction.Input, Port.Capacity.Single);
                input.portColor = Color.red;

                //Output Port Data
                break;
            case DialogueNode:
                //Input Port Data
                input = GeneratePort(node, Direction.Input, Port.Capacity.Multi);

                //Output Port Data

                var button = new Button (() => {AddChoicePort(node);});
                button.text = "New Choice";
                titleContainer.Add(button);
                break;
        }

        if (input != null)
        {
            input.portName = "Input";
            inputContainer.Add(input);
        }

        if (output != null)
        {
            output.portName = "Output";
            outputContainer.Add(output);
        }

        Debug.Log(output);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    public void AddChoicePort(Node node, string overridenPortName = "")
    {
        var generatedPort = GeneratePort(node, Direction.Output, Port.Capacity.Single);
        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);
        var outputPortCount = outputContainer.Query("connector").ToList().Count;
        var choicePortName = string.IsNullOrEmpty(overridenPortName) 
            ? $"Choice {outputPortCount}"
            : overridenPortName;
        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(node, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);
        generatedPort.portName = choicePortName;
        outputContainer.Add(generatedPort);
        RefreshPorts();
        RefreshExpandedState();
    }

    private void RemovePort(Node node, Port generatedPort)
    {
        //var targetEdge = output.ConnectTo(input).ToList().Where(x => 
        //x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);
//
        //if (targetEdge.Any()) 
        //{
        //    var edge = targetEdge.First();
        //    edge.input.Disconnect(edge);
        //    outputContainer.Remove(targetEdge.First());
        //}

        Remove(generatedPort);
        RefreshPorts();
        RefreshExpandedState();
    }
}
