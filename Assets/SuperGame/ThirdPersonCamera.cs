using UnityEngine;
using UnityEngine.InputSystem;

namespace SuperGame
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] Transform _target;

        ThirdPersonCameraSettings _settings;
        float _yaw;
        float _pitch;

        public bool ControlEnabled { get; set; } = true;
        public float Yaw => _yaw;

        public void Init(GameSettings settings)
        {
            _settings = settings?.CameraSettings;
        }

        private void Start()
        {
            if (_target is null)
                return;

            var angles = transform.eulerAngles;
            _yaw = angles.y;
            _pitch = angles.x;
        }

        private void Update()
        {
            if (_target is null || _settings is null)
                return;

            if (ControlEnabled && Mouse.current is not null)
            {
                var delta = Mouse.current.delta.ReadValue();
                _yaw += delta.x * _settings.Sensitivity;
                _pitch -= delta.y * _settings.Sensitivity;
                _pitch = Mathf.Clamp(_pitch, _settings.MinPitch, _settings.MaxPitch);
            }

            var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            var targetPosition = _target.position + Vector3.up * _settings.HeightOffset;
            var desiredPosition = targetPosition - rotation * Vector3.forward * _settings.Distance;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, _settings.FollowSmoothing * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _settings.RotationSmoothing * Time.deltaTime);
        }
    }
}
