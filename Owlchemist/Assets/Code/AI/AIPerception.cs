using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AIPerception : MonoBehaviour
{
    public float perceptionRadius = 1f;
    [Range(0, 360f)] public float fovAngle = 100f;
    public LayerMask perceptionMask;
    public Material seenMaterial;
    public Material unseenMaterial;
    public List<Collider> previouslySeen = new List<Collider>();
    public ChickenStateMachine chickenStateMachine;

    private void Awake()
    {
        chickenStateMachine = GetComponent<ChickenStateMachine>();
        Assert.IsNotNull(chickenStateMachine, "<b>AIPerception</b> failed to locate <b>ChickenStateMachine</b> on Enemy GameObject");
    }

    private void Update()
    {
        for (int i = 0; i < previouslySeen.Count; i++)
        {
            // material renderer for sphere above player. Debug reasons only!
            previouslySeen[i].GetComponent<Renderer>().sharedMaterial = unseenMaterial;
            chickenStateMachine.seeingPlayer = false;
        }


        previouslySeen.Clear();

        previouslySeen.AddRange(Physics.OverlapSphere(transform.position, perceptionRadius, perceptionMask, QueryTriggerInteraction.Ignore));
        for (int i = 0; i < previouslySeen.Count; i++)
        {
            Vector3 direction = previouslySeen[i].transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle <= fovAngle * 0.5f)
            {
                // material renderer for sphere above player. Debug reasons only!
                previouslySeen[i].GetComponent<Renderer>().sharedMaterial = seenMaterial;
                chickenStateMachine.seeingPlayer = true;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Color oldColor = Gizmos.color;
            float halfFOV = fovAngle * 0.5f;
            float coneDir = -90f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV + coneDir, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV + coneDir, Vector3.up);
            Vector3 leftDir = leftRayRotation * transform.right * perceptionRadius;
            Vector3 rightDir = rightRayRotation * transform.right * perceptionRadius;

            UnityEditor.Handles.color = new Color(0f, 1f, 0f, 0.15f);
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, leftDir, fovAngle, perceptionRadius);
            UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.15f);
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rightDir, 360f - fovAngle, perceptionRadius);

            Gizmos.color = oldColor;
        }
    }
#endif
}
