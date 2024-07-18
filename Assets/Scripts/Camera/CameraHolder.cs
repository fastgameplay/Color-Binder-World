using UnityEngine;
using Utils.Singleton;
using Player.ActionHandlers;
using Unity.VisualScripting;
using Events;
using Utils.Scenes;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        [HideInInspector] public Vector2 ScreenBoundaryMin = new Vector2(-10, -10);
        [HideInInspector] public Vector2 ScreenBoundaryMax = new Vector2(10, 10);
        public UnityEngine.Camera MainCamera => mainCamera;
        public bool ShowGizmos => cameraConfig.ShowGizmos;

        [Header("References")]
        [SerializeField] private UnityEngine.Camera mainCamera;
        [Header("Config")]
        [SerializeField] private SO_CameraConfig cameraConfig;
        
        private float swipeSensitivity => cameraConfig.SwipeSensitivity;
        private float cameraMoveDistance => cameraConfig.CameraMoveDistance;
        private float cameraMoveSpeed => cameraConfig.CameraMoveSpeed;

        private Vector2 adjustedBoundaryMin, adjustedBoundaryMax;
        private Vector3 startDragPosition, endDragPosition, targetPosition;
        private bool isActive;

        private void Start()
        {
            isActive = true;
            targetPosition = mainCamera.transform.position;
            UpdateAdjustedBoundaries();
            EventsController.Subscribe<EventModels.Game.NodeTapped>(this,OnNodeTapped);
            EventsController.Subscribe<EventModels.Game.PlayerFingerRemoved>(this,OnNodeReleased);
            ScenesChanger.SceneLoadedEvent += SubscribeToClickHandler;

        }
        private void LateUpdate()
        {
            // if(!isActive) return;
            if(mainCamera.transform.position == targetPosition) return;
            if(Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.01f){
                mainCamera.transform.position = targetPosition;
            }
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraMoveSpeed * Time.fixedDeltaTime);
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
        private void OnNodeTapped(EventModels.Game.NodeTapped nodeTapped) => isActive = false;
        private void OnNodeReleased(EventModels.Game.PlayerFingerRemoved playerFingerRemoved) => isActive = true;
        private void SubscribeToClickHandler() => ClickHandler.Instance.SetDragEventHandlers(OnDragStart, OnDragEnd, false);

        private void HandleSwipe()
        {
            if(!isActive) return;
            Vector3 direction = endDragPosition - startDragPosition;

            if (direction.magnitude > swipeSensitivity)
            {
                direction.Normalize();
                UpdateTargetPosition(direction);
            }
        }

        public void UpdateTargetPosition(Vector3 direction)
        {
            // Calculate new target position based on direction and move speed
            Vector3 movement = new Vector3(direction.x, direction.y, 0) * cameraMoveDistance;
            targetPosition = mainCamera.transform.position + movement;

            // Clamp the target position within boundaries
            targetPosition.x = Mathf.Clamp(targetPosition.x, adjustedBoundaryMin.x, adjustedBoundaryMax.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, adjustedBoundaryMin.y, adjustedBoundaryMax.y);
            targetPosition.z = mainCamera.transform.position.z;
        }
        public void UpdateAdjustedBoundaries()
        {
            float cameraHeight = MainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * MainCamera.aspect;

            adjustedBoundaryMin = new Vector2(ScreenBoundaryMin.x + cameraWidth / 2, ScreenBoundaryMin.y + cameraHeight / 2);
            adjustedBoundaryMax = new Vector2(ScreenBoundaryMax.x - cameraWidth / 2, ScreenBoundaryMax.y - cameraHeight / 2);

        }
    }
}
