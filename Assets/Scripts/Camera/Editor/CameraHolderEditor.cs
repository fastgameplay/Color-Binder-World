using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Camera.CameraHolder))]
public class CameraHolderEditor : Editor
{
    private void OnSceneGUI()
    {
        Camera.CameraHolder cameraHolder = (Camera.CameraHolder)target;

        if (!cameraHolder.ShowGizmos) return;

        Vector3 bottomLeft = new Vector3(cameraHolder.BoundaryMin.x, cameraHolder.BoundaryMin.y, cameraHolder.MainCamera.transform.position.z);
        Vector3 topRight = new Vector3(cameraHolder.BoundaryMax.x, cameraHolder.BoundaryMax.y, cameraHolder.MainCamera.transform.position.z);

        Handles.color = Color.red;
        Handles.DrawLine(bottomLeft, new Vector3(bottomLeft.x, topRight.y, cameraHolder.MainCamera.transform.position.z));
        Handles.DrawLine(new Vector3(bottomLeft.x, topRight.y, cameraHolder.MainCamera.transform.position.z), topRight);
        Handles.DrawLine(topRight, new Vector3(topRight.x, bottomLeft.y, cameraHolder.MainCamera.transform.position.z));
        Handles.DrawLine(new Vector3(topRight.x, bottomLeft.y, cameraHolder.MainCamera.transform.position.z), bottomLeft);

        EditorGUI.BeginChangeCheck();
        Vector3 newBottomLeft = Handles.PositionHandle(bottomLeft, Quaternion.identity);
        Vector3 newTopRight = Handles.PositionHandle(topRight, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cameraHolder, "Change Camera Boundary");

            cameraHolder.BoundaryMin = new Vector2(newBottomLeft.x, newBottomLeft.y);
            cameraHolder.BoundaryMax = new Vector2(newTopRight.x, newTopRight.y);

            cameraHolder.BoundaryMin = new Vector2(Mathf.Min(cameraHolder.BoundaryMin.x, cameraHolder.BoundaryMax.x), Mathf.Min(cameraHolder.BoundaryMin.y, cameraHolder.BoundaryMax.y));
            cameraHolder.BoundaryMax = new Vector2(Mathf.Max(cameraHolder.BoundaryMin.x, cameraHolder.BoundaryMax.x), Mathf.Max(cameraHolder.BoundaryMin.y, cameraHolder.BoundaryMax.y));
        }
    }
}
