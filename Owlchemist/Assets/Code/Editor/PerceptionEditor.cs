using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(AIPerception))]
public class PerceptionEditor : Editor
{
    private ArcHandle arcHandle = new ArcHandle();

    private void OnEnable()
    {
        arcHandle.fillColor = new Color(0f, 1, 0f, 0.15f);
        arcHandle.wireframeColor = arcHandle.fillColor;
        arcHandle.angleHandleColor = Color.red;
        arcHandle.radiusHandleColor = Color.red;
    }

    private void OnSceneGUI()
    {
        if (!target)
            return;

        Color oldColor = Handles.color;
        AIPerception perception = target as AIPerception;

        float halfFOV = perception.fovAngle * 0.5f;
        float coneDir = -90f;

        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV + coneDir, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV + coneDir, Vector3.up);

        Vector3 leftRayDir = leftRayRotation * perception.transform.right * perception.perceptionRadius;
        Vector3 rightRayDir = rightRayRotation * perception.transform.right * perception.perceptionRadius;

        arcHandle.angle = perception.fovAngle;
        arcHandle.radius = perception.perceptionRadius;

        Vector3 handleDir = leftRayDir;
        Vector3 handleNormal = Vector3.Cross(handleDir, perception.transform.forward);
        Matrix4x4 handleMatrix = Matrix4x4.TRS(
            perception.transform.position,
            Quaternion.LookRotation(handleDir, handleNormal),
            Vector3.one);

        using (new Handles.DrawingScope(handleMatrix))
        {
            EditorGUI.BeginChangeCheck();
            arcHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(perception, "Changed perception properties");
                perception.fovAngle = Mathf.Clamp(arcHandle.angle, 0f, 359f);
                perception.perceptionRadius = Mathf.Max(arcHandle.radius, 1f);
            }
        }

        Handles.color = new Color(1f, 0f, 0f, 0.15f);
        Handles.DrawSolidArc(perception.transform.position, Vector3.up, rightRayDir, 360f - perception.fovAngle, perception.perceptionRadius);
        Handles.color = oldColor;
    }
}
