using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Encounter)), CanEditMultipleObjects]
public class DrawWireArcEncounter : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        Encounter enc = (Encounter)target;
        Handles.DrawWireArc(enc.transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 360f, enc.Range);
        Handles.DrawDottedLine(enc.transform.position, enc.transform.position + new Vector3(0, enc.Range, 0), 12f);
        Handles.DrawDottedLine(enc.transform.position, enc.transform.position + new Vector3(enc.Range, 0, 0), 12f);
    }
}