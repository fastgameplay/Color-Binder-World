using UnityEngine;
using Utils.Singleton;
using Player.ActionHandlers;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        public UnityEngine.Camera MainCamera => mainCamera;
        [HideInInspector] public Vector2 BoundaryMin = new Vector2(-10, -10);
        [HideInInspector] public Vector2 BoundaryMax = new Vector2(10, 10);
        [field: SerializeField] public bool ShowGizmos {get; private set;} = true;

        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private float swipeSensitivity = 0.5f;
        [SerializeField] private float cameraMoveSpeed = 10f;

        private Vector3 startDragPosition;
        private Vector3 endDragPosition;
        [HideInInspector] private Vector2 adjustedBoundaryMin;
        [HideInInspector] private Vector2 adjustedBoundaryMax;

        private void Start()
        {
            // Set up the drag event handlers from ClickHandler
            ClickHandler.Instance.SetDragEventHandlers(OnDragStart, OnDragEnd);
        }
        private void OnValidate()
        {
            UpdateAdjustedBoundaries();
        }
        private void OnDragStart(Vector3 startPosition)
        {
            startDragPosition = startPosition;
        }

        private void OnDragEnd(Vector3 endPosition)
        {
            endDragPosition = endPosition;
            HandleSwipe();
        }

        private void HandleSwipe()
        {
            Vector3 direction = endDragPosition - startDragPosition;

            if (direction.magnitude > swipeSensitivity)
            {
                direction.Normalize();
                MoveCameraInDirection(direction);
            }
        }
        private void UpdateAdjustedBoundaries()
        {
            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            adjustedBoundaryMin = new Vector2(BoundaryMin.x + cameraWidth / 2, BoundaryMin.y + cameraHeight / 2);
            adjustedBoundaryMax = new Vector2(BoundaryMax.x - cameraWidth / 2, BoundaryMax.y - cameraHeight / 2);
        }
        private void MoveCameraInDirection(Vector3 direction)
        {
            Vector3 movement = new Vector3(direction.x, direction.y, 0) * cameraMoveSpeed;
            Vector3 newPosition = mainCamera.transform.position + movement;

            // Clamp the camera's position to stay within the boundary
            newPosition.x = Mathf.Clamp(newPosition.x, adjustedBoundaryMin.x, adjustedBoundaryMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, adjustedBoundaryMin.y, adjustedBoundaryMax.y);

            mainCamera.transform.position = newPosition;
        }
    }
}
