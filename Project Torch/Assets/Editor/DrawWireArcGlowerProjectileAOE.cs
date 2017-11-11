using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This little bit here draws lines and arcs visually showing the radius
/// </summary>
[CustomEditor(typeof(GlowerProjectileAOE)), CanEditMultipleObjects]
public class DrawWireArcGlowerProjectileAOE : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        GlowerProjectileAOE aoe = (GlowerProjectileAOE)target;
        Handles.DrawWireArc(aoe.transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 360f, aoe.radius);
        Handles.DrawDottedLine(aoe.transform.position, aoe.transform.position + new Vector3(0, aoe.radius, 0), 12f);
        Handles.DrawDottedLine(aoe.transform.position, aoe.transform.position + new Vector3(aoe.radius, 0, 0), 12f);
    }
}