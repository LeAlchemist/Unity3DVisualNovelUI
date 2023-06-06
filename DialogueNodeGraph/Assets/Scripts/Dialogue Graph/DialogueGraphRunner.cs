using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGraphRunner : MonoBehaviour
{
    public DialogueGraph graph;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        graph.Update();
    }
}
