using UnityEngine;
using UnityEngine.InputSystem;

namespace SuperGame
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "SuperGame/Game Settings")]
    public class GameSettings : ScriptableObject
    {

        public InputActionAsset InputActions;
        public ThirdPersonCameraSettings CameraSettings;
        public MovementSettings Movement;

        [Range(-1f, 1f)]
        [SerializeField] float _pickupMinDot = 0.7f;
        [SerializeField] float _pickupRange = 3f;

        public float PickupMinDot => _pickupMinDot;
        public float PickupRange => _pickupRange;
    }
}
