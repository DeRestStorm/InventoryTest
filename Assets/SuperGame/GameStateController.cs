using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace SuperGame
{
    public enum GameStateType
    {
        Gameplay,
        Inventory
    }

    public class GameStateController : MonoBehaviour
    {
        [SerializeField] ThirdPersonCamera _camera;
        [SerializeField] PlayerMovement _playerMovement;
        [SerializeField] InventoryPanel _inventoryPanel;
        [SerializeField] UIDocument _takeUI;

        GameStateType _current = GameStateType.Gameplay;

        private void OnEnable()
        {
            _inventoryPanel.OnToggled += HandleInventoryToggled;
            ApplyState();
        }

        private void OnDisable()
        {
            _inventoryPanel.OnToggled -= HandleInventoryToggled;
        }

        private void HandleInventoryToggled(bool visible)
        {
            SetState(visible ? GameStateType.Inventory : GameStateType.Gameplay);
        }

        private void SetState(GameStateType state)
        {
            if (_current == state)
                return;

            _current = state;
            ApplyState();
        }

        private void ApplyState()
        {
            bool gameplay = _current == GameStateType.Gameplay;

            _camera.ControlEnabled = gameplay;
            _playerMovement.ControlEnabled = gameplay;

            if (_takeUI is not null)
                _takeUI.rootVisualElement.style.display = gameplay ? DisplayStyle.Flex : DisplayStyle.None;

            Cursor.lockState = gameplay ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = gameplay is false;
        }
    }
}
