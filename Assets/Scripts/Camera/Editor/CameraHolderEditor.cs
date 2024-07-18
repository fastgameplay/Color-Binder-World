using UnityEditor;
using UnityEngine;
namespace Camera
{
    [CustomEditor(typeof(CameraHolder))]
    public class CameraHolderEditor : Editor
    {
        private void OnSceneGUI()
        {
            CameraHolder cameraHolder = (CameraHolder)target;

            if (!cameraHolder.ShowGizmos) return;

            Vector3 bottomLeft = new Vector3(cameraHolder.ScreenBoundaryMin.x, cameraHolder.ScreenBoundaryMin.y, cameraHolder.MainCamera.transform.position.z);
            Vector3 topRight = new Vector3(cameraHolder.ScreenBoundaryMax.x, cameraHolder.ScreenBoundaryMax.y, cameraHolder.MainCamera.transform.position.z);

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

                cameraHolder.ScreenBoundaryMin = new Vector2(newBottomLeft.x, newBottomLeft.y);
                cameraHolder.ScreenBoundaryMax = new Vector2(newTopRight.x, newTopRight.y);

                cameraHolder.ScreenBoundaryMin = new Vector2(Mathf.Min(cameraHolder.ScreenBoundaryMin.x, cameraHolder.ScreenBoundaryMax.x), Mathf.Min(cameraHolder.ScreenBoundaryMin.y, cameraHolder.ScreenBoundaryMax.y));
                cameraHolder.ScreenBoundaryMax = new Vector2(Mathf.Max(cameraHolder.ScreenBoundaryMin.x, cameraHolder.ScreenBoundaryMax.x), Mathf.Max(cameraHolder.ScreenBoundaryMin.y, cameraHolder.ScreenBoundaryMax.y));

                UpdateAdjustedBoundaries(cameraHolder);
            }
        }

        private void UpdateAdjustedBoundaries(CameraHolder cameraHolder)
        {
            float cameraHeight = cameraHolder.MainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * cameraHolder.MainCamera.aspect;

            cameraHolder.AdjustedBoundaryMin = new Vector2(cameraHolder.ScreenBoundaryMin.x + cameraWidth / 2, cameraHolder.ScreenBoundaryMin.y + cameraHeight / 2);
            cameraHolder.AdjustedBoundaryMax = new Vector2(cameraHolder.ScreenBoundaryMax.x - cameraWidth / 2, cameraHolder.ScreenBoundaryMax.y - cameraHeight / 2);

        }
    }
}