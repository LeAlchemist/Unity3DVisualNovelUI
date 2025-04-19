using System;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public bool onStart, onStop, onUpdate;
    public string message;

    protected override void OnStart()
    {
        if (onStart)
        {
            Debug.Log($"OnStart {message}");
        }
    }

    protected override void OnStop()
    {
        if (onStop)
        {
            Debug.Log($"OnStop {message}");
        }
    }

    protected override State OnUpdate()
    {
        if (onUpdate)
        {
            Debug.Log($"OnUpdate {message}");
        }
        return State.Success;
    }
}
