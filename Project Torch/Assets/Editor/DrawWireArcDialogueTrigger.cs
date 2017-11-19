using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTrigger)), CanEditMultipleObjects]
public class DrawWireArcDialogueTrigger : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        DialogueTrigger trigger = (DialogueTrigger)target;
        Handles.DrawWireArc(trigger.transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 360f, trigger.range);
        Handles.DrawDottedLine(trigger.transform.position, trigger.transform.position + new Vector3(0, trigger.range, 0), 12f);
        Handles.DrawDottedLine(trigger.transform.position, trigger.transform.position + new Vector3(trigger.range, 0, 0), 12f);
    }
}