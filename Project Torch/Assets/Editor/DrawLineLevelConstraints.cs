using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This little bit here draws lines visually showing the level constraints
/// </summary>
[CustomEditor(typeof(Zone)), CanEditMultipleObjects]
public class DrawLineLevelConstraints : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        Zone z = (Zone)target;
        Handles.DrawLine(new Vector3(z.minX, 200f, 0f), new Vector3(z.minX, -200f, 0f));
        Handles.DrawLine(new Vector3(z.maxX, 200f, 0f), new Vector3(z.maxX, -200f, 0f));
        Handles.DrawLine(new Vector3(200f, z.minY, 0f), new Vector3(-200f, z.minY, 0f));
        Handles.DrawLine(new Vector3(200f, z.maxY, 0f), new Vector3(-200f, z.maxY, 0f));
    }
}