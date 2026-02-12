using UnityEngine;
using UnityEngine.InputSystem;

namespace SuperGame
{
    [CreateAssetMenu(fileName = "GameInputSettings", menuName = "SuperGame/Game Input Settings")]
    public class GameInputSettings : ScriptableObject
    {
        [System.Serializable]
        public class MovementSettings
        {
            public float MoveSpeed = 5f;
            public float RotationSpeed = 10f;
            public float Gravity = -20f;
        }

        public const string ResourcePath = "GameInputSettings";

        public InputActionAsset InputActions;
        public ThirdPersonCameraSettings CameraSettings;
        public MovementSettings Movement;

        [Range(-1f, 1f)]
        [SerializeField] float _pickupMinDot = 0.7f;

        public float PickupMinDot => _pickupMinDot;
    }
}
