using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIWaypointSystem))]
public class WaypointEditor :  Editor
{
    private Tool lastTool;
    private GUIStyle style = new GUIStyle();

    private void OnEnable()
    {
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        lastTool = Tools.current;
        Tools.current = Tool.None;
    }

    private void OnDisable()
    {
        Tools.current = lastTool;
    }

    private void OnSceneGUI()
    {
        AIWaypointSystem wayPoint = (AIWaypointSystem)target;

        for (int i = 0; i < wayPoint.wayPoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Handles.Label(wayPoint.wayPoints[i], i.ToString(), style);
            Vector3 newPosition = Handles.PositionHandle(wayPoint.wayPoints[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(wayPoint, "Waypoint moved");
                wayPoint.wayPoints[i] = newPosition;
            }
        }
    }
}
