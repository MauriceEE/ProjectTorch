using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This little bit here draws lines and arcs visually showing the visibility range
/// Strangely, you have to specify explicitly to make multiple objects editable
/// </summary>
[CustomEditor(typeof(Enemy)), CanEditMultipleObjects]
public class DrawWireArcEnemy : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        Enemy enemy = (Enemy)target;
        Handles.DrawWireArc(enemy.transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 360f, enemy.awarenessRange);
        Handles.DrawDottedLine(enemy.transform.position, enemy.transform.position + new Vector3(0, enemy.awarenessRange, 0), 12f);
        Handles.DrawDottedLine(enemy.transform.position, enemy.transform.position + new Vector3(enemy.awarenessRange, 0, 0), 12f);
    }
}