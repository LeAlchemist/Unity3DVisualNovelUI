using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    public string GUID;
    public string NodeType;
    [TextArea]
    public string DialogueText;
    public Vector2 Position;
}
