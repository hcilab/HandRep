using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;

[CustomEditor(typeof(XRPropertySocket), true), CanEditMultipleObjects]
public class SocketEditor : XRSocketInteractorEditor
{

    protected SerializedProperty targetProperties;

    protected override void OnEnable()
    {
        base.OnEnable();
        targetProperties = serializedObject.FindProperty("targetProperties");
    }

    protected override void DrawProperties()
    {
        base.DrawProperties();
        EditorGUILayout.PropertyField(targetProperties);
    }
}
