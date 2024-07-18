using UnityEngine;
using Utils.Singleton;
using Player.ActionHandlers;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        public UnityEngine.Camera MainCamera => mainCamera;
        public Vector2 boundaryMin = new Vector2(-10, -10);
        public Vector2 boundaryMax = new Vector2(10, 10);
        [field: SerializeField] public bool showGizmos {get; private set;} = true;

        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private float swipeSensitivity = 0.5f;
        [SerializeField] private float cameraMoveSpeed = 10f;

        private Vector3 _startDragPosition;
        private Vector3 _endDragPosition;


        private void Start()
        {
            // Set up the drag event handlers from ClickHandler
            ClickHandler.Instance.SetDragEventHandlers(OnDragStart, OnDragEnd);
        }

        private void OnDragStart(Vector3 startPosition)
        {
            _startDragPosition = startPosition;
        }

        private void OnDragEnd(Vector3 endPosition)
        {
            _endDragPosition = endPosition;
            HandleSwipe();
        }

        private void HandleSwipe()
        {
            Vector3 direction = _endDragPosition - _startDragPosition;

            if (direction.magnitude > swipeSensitivity)
            {
                direction.Normalize();
                MoveCameraInDirection(direction);
            }
        }

        private void MoveCameraInDirection(Vector3 direction)
        {
            Vector3 move = new Vector3(direction.x, direction.y, 0) * cameraMoveSpeed * Time.deltaTime;
            Vector3 newPosition = mainCamera.transform.position + move;

            // Clamp the camera's position to stay within the boundary
            newPosition.x = Mathf.Clamp(newPosition.x, boundaryMin.x, boundaryMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, boundaryMin.y, boundaryMax.y);

            mainCamera.transform.position = newPosition;
        }
    }
}
