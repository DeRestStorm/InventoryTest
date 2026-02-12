using UnityEngine;
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
        ThirdPersonCamera _camera;
        PlayerMovement _playerMovement;
        InventoryPanel _inventoryPanel;
        PickupSystem _pickupSystem;
        GameStateType _current = GameStateType.Gameplay;

        public void Init(ThirdPersonCamera camera, PlayerMovement playerMovement, InventoryPanel inventoryPanel, PickupSystem pickupSystem)
        {
            _camera = camera;
            _playerMovement = playerMovement;
            _inventoryPanel = inventoryPanel;
            _pickupSystem = pickupSystem;
            if (isActiveAndEnabled)
                SubscribeAndApply();
        }

        private void OnEnable()
        {
            if (_inventoryPanel != null)
                SubscribeAndApply();
        }

        private void OnDisable()
        {
            if (_inventoryPanel != null)
                _inventoryPanel.OnToggled -= HandleInventoryToggled;
        }

        private void SubscribeAndApply()
        {
            _inventoryPanel.OnToggled += HandleInventoryToggled;
            ApplyState();
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
            _pickupSystem.SetGameplayUIVisible(gameplay);

            Cursor.lockState = gameplay ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = gameplay is false;
        }
    }
}
