using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Camera.CameraHolder))]
public class CameraHolderEditor : Editor
{
    private void OnSceneGUI()
    {
        Camera.CameraHolder cameraHolder = (Camera.CameraHolder)target;

        if (!cameraHolder.showGizmos) return;

        Vector3 bottomLeft = new Vector3(cameraHolder.boundaryMin.x, cameraHolder.boundaryMin.y, cameraHolder.MainCamera.transform.position.z);
        Vector3 topRight = new Vector3(cameraHolder.boundaryMax.x, cameraHolder.boundaryMax.y, cameraHolder.MainCamera.transform.position.z);

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

            cameraHolder.boundaryMin = new Vector2(newBottomLeft.x, newBottomLeft.y);
            cameraHolder.boundaryMax = new Vector2(newTopRight.x, newTopRight.y);

            cameraHolder.boundaryMin = new Vector2(Mathf.Min(cameraHolder.boundaryMin.x, cameraHolder.boundaryMax.x), Mathf.Min(cameraHolder.boundaryMin.y, cameraHolder.boundaryMax.y));
            cameraHolder.boundaryMax = new Vector2(Mathf.Max(cameraHolder.boundaryMin.x, cameraHolder.boundaryMax.x), Mathf.Max(cameraHolder.boundaryMin.y, cameraHolder.boundaryMax.y));
        }
    }
}
