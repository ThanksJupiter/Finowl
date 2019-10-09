using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShaderVariable))]
public class ShaderVariableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShaderVariable sv = target as ShaderVariable;

        if (GUILayout.Button("Apply color"))
        {
            sv.SetColor();    
        }

        EditorGUI.BeginChangeCheck();

        sv.color = EditorGUILayout.ColorField(sv.color);

        if (EditorGUI.EndChangeCheck())
        {
            sv.SetColor();
        }

        base.OnInspectorGUI();
    }
}
