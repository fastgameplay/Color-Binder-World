using UnityEngine;
using Utils.Singleton;
using Player.ActionHandlers;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        public UnityEngine.Camera MainCamera => mainCamera;
        [HideInInspector] public Vector2 ScreenBoundaryMin = new Vector2(-10, -10);
        [HideInInspector] public Vector2 ScreenBoundaryMax = new Vector2(10, 10);
        [HideInInspector] public Vector2 AdjustedBoundaryMin = new Vector2(10, 0);
        [HideInInspector] public Vector2 AdjustedBoundaryMax;
        [field: SerializeField] public bool ShowGizmos {get; private set;} = true;

        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private float swipeSensitivity = 0.5f;
        [SerializeField] private float cameraMoveSpeed = 10f;

        private Vector3 startDragPosition;
        private Vector3 endDragPosition;

        private void Start()
        {
            // Set up the drag event handlers from ClickHandler
            ClickHandler.Instance.SetDragEventHandlers(OnDragStart, OnDragEnd);
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

        private void MoveCameraInDirection(Vector3 direction)
        {
            Vector3 movement = new Vector3(direction.x, direction.y, 0) * cameraMoveSpeed;
            Vector3 newPosition = mainCamera.transform.position + movement;

            // Clamp the camera's position to stay within the boundary
            newPosition.x = Mathf.Clamp(newPosition.x, AdjustedBoundaryMin.x, AdjustedBoundaryMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, AdjustedBoundaryMin.y, AdjustedBoundaryMax.y);

            mainCamera.transform.position = newPosition;
        }
    }
}
