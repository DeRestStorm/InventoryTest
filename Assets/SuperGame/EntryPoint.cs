using System.Linq;
using Definitions;
using SuperGame.ECS;
using UnityEngine;

namespace SuperGame
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] InventoryPanel _inventoryPanel;
        [SerializeField] PickupSystem _pickupSystem;
        [SerializeField] WorldItem _worldItemPrefab;
        [SerializeField] Transform _player;
        [SerializeField] GameInputSettings _gameSettings;
        [SerializeField] PlayerMovement _playerMovement;
        [SerializeField] ThirdPersonCamera _thirdPersonCamera;
        [SerializeField] GameStateController _gameStateController;

        DefData _defData;
        PlayerState _playerState;
        Commands _commands;

        private void Start()
        {
            _defData = InitDefs.LoadFromJson();
            var worldItems = FindObjectsByType<WorldItem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var world = new World();
            InitWorldItem(worldItems, world);
            var worldApi = new WorldApi(world, _worldItemPrefab, _defData, _player);

            _playerState = new PlayerState();
            _commands = new Commands(_defData, _playerState.Inventory, worldApi);

            _inventoryPanel.Init(_playerState.Inventory, _commands, _defData);
            _pickupSystem.Init(_commands, _gameSettings);
            _playerMovement.Init(_gameSettings);
            _thirdPersonCamera.Init(_gameSettings);
            _gameStateController.Init(_thirdPersonCamera, _playerMovement, _inventoryPanel, _pickupSystem);
        }

        private void InitWorldItem(WorldItem[] worldItems, World world)
        {
            foreach (var worldItem in worldItems)
            {
                var defId = new ItemDefId(worldItem.DefId);
                if (_defData.Items.TryGetValue(defId, out var def) is false)
                {
                    Debug.LogError($"Item {defId} not found in defs.Items");
                    continue;
                }

                var itemId = world.AllocateItemId();

                var state = new ItemState(itemId, defId, worldItem.Count);
                worldItem.Init(state, def);
                world.WorldItems.Add(itemId, worldItem);
            }
        }
    }
}