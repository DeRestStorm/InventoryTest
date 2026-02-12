using UnityEngine;
using UnityEngine.InputSystem;

namespace SuperGame
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] ThirdPersonCamera _camera;

        CharacterController _controller;
        InputAction _moveAction;
        MovementSettings _movementSettings;
        float _verticalVelocity;

        public bool ControlEnabled { get; set; } = true;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void Init(GameSettings settings)
        {
            if (settings is null || settings.InputActions is null)
                return;

            _movementSettings = settings.Movement;

            var map = settings.InputActions.FindActionMap("Player");
            map.Enable();
            _moveAction = map.FindAction("Move");
        }

        private void Update()
        {
            if (_moveAction is null || ControlEnabled is false || _camera is null || _movementSettings is null)
                return;

            ApplyGravity();

            var input = _moveAction.ReadValue<Vector2>();
            var velocity = new Vector3(0f, _verticalVelocity, 0f);

            if (input.sqrMagnitude > 0.01f)
            {
                var cameraYaw = Quaternion.Euler(0f, _camera.Yaw, 0f);
                var moveDirection = cameraYaw * new Vector3(input.x, 0f, input.y);

                velocity += moveDirection * _movementSettings.MoveSpeed;

                var targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _movementSettings.RotationSpeed * Time.deltaTime);
            }

            _controller.Move(velocity * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (_movementSettings is null)
                return;

            if (_controller.isGrounded)
                _verticalVelocity = -1f;
            else
                _verticalVelocity += _movementSettings.Gravity * Time.deltaTime;
        }
    }
}
