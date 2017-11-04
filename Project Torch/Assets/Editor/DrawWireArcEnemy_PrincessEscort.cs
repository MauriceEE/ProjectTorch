using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This little bit here draws lines and arcs visually showing the player aggro range
/// </summary>
[CustomEditor(typeof(Enemy_PrincessEscort)), CanEditMultipleObjects]
public class DrawWireArcEnemy_PrincessEscort : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        Enemy_PrincessEscort enemy = (Enemy_PrincessEscort)target;
        Handles.DrawWireArc(enemy.transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 360f, enemy.playerAggroDist);
        Handles.DrawDottedLine(enemy.transform.position, enemy.transform.position + new Vector3(0, enemy.playerAggroDist, 0), 12f);
        Handles.DrawDottedLine(enemy.transform.position, enemy.transform.position + new Vector3(enemy.playerAggroDist, 0, 0), 12f);
    }
}