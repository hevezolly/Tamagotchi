using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PuffBallVisual))]
public class meshButton : Editor
{
    public override void OnInspectorGUI()
    {
        var v = (PuffBallVisual)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("generate Mesh"))
        {
            v.BuildMeshEditor();
        }
    }
}
