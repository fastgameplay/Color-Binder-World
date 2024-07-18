using UnityEngine;
namespace Camera
{
    [CreateAssetMenu(fileName = "Camera Config", menuName = "Configs/Camera")]
    public class SO_CameraConfig : ScriptableObject
    {
        [field: SerializeField] public bool ShowGizmos {get; private set;} = true;
        [field: SerializeField] public float SwipeSensitivity {get; private set;} = 0.5f;
        [field: SerializeField] public float CameraMoveDistance {get; private set;} = 10f;
        [field: SerializeField] public float CameraMoveSpeed {get; private set;} = 0.5f;

    }
}