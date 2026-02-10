using UnityEngine;
using UnityEngine.InputSystem;

namespace SuperGame
{
    public class GameInputSettings : ScriptableObject
    {
        public const string ResourcePath = "GameInputSettings";

        public InputActionAsset InputActions;

        InputAction _moveAction;

        public InputAction MoveAction
        {
            get
            {
                if (_moveAction is null)
                    Init();
                return _moveAction;
            }
        }

        void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            if (InputActions is null)
                return;
            var map = InputActions.FindActionMap("Player");
            map.Enable();
            _moveAction = map.FindAction("Move");
        }
    }
}
