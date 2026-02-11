using UnityEngine;
using UnityEngine.InputSystem;
using SuperGame;

namespace SuperGame
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float MoveSpeed = 5f;

        InputAction _moveAction;

        void Awake()
        {
            var settings = Resources.Load<GameInputSettings>(GameInputSettings.ResourcePath);
            if (settings is null || settings.InputActions is null)
                return;
            var map = settings.InputActions.FindActionMap("Player");
            map.Enable();
            _moveAction = map.FindAction("Move");
        }

        void Update()
        {
            if (_moveAction is null)
                return;
            var v = _moveAction.ReadValue<Vector2>();
            var move = new Vector3(v.x, 0f, v.y) * (MoveSpeed * Time.deltaTime);
            transform.position += move;
        }
    }
}
