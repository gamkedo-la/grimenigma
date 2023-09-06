using UnityEditor;
using UnityEngine;
using UnityEngine.Accessibility;

[CustomEditor(typeof(EnemyVision))]
public class EnemyVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyVision vision = (EnemyVision)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(vision.transform.position, Vector3.up, Vector3.forward, 360, vision.radius);

        Vector3 viewAngle01 = DirectionFromAngle(vision.transform.eulerAngles.y, -vision.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(vision.transform.eulerAngles.y, vision.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngle01 * vision.radius);
        Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngle02 * vision.radius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0 , Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
