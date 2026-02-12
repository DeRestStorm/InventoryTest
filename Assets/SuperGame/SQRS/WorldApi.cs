using Definitions;
using SuperGame.ECS;
using UnityEngine;

namespace SuperGame
{
    public class WorldApi
    {
        private readonly World _world;
        private readonly WorldItem _itemPrefab;
        private readonly DefData _defData;
        private readonly Transform _player;
        private const float DropDistance = 1.5f;

        public WorldApi(World world, WorldItem itemPrefab, DefData defData, Transform player)
        {
            _world = world;
            _itemPrefab = itemPrefab;
            _defData = defData;
            _player = player;
        }

        public bool TryGetItem(int itemId, out ItemState itemState)
        {
            if (_world.WorldItems.TryGetValue(itemId, out var worldItem) is false)
            {
                itemState = null;
                return false;
            }

            itemState = worldItem.ItemState;
            return true;
        }

        public void UpdateItemCount(int itemId, int newCount)
        {
            if (_world.WorldItems.TryGetValue(itemId, out var worldItem) is false)
                return;

            worldItem.ItemState.Count = newCount;
        }

        public void DestroyItem(int itemId)
        {
            if (_world.WorldItems.TryGetValue(itemId, out var worldItem) is false)
                return;

            Object.Destroy(worldItem.gameObject);
            _world.WorldItems.Remove(itemId);
        }

        public void SpawnItem(ItemState inventoryItems)
        {
            if (_itemPrefab is null || _defData.Items.TryGetValue(inventoryItems.DefId, out var def) is false)
                return;

            var dropOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            var position = _player.position + (dropOffset * DropDistance);
            var worldItem = Object.Instantiate(_itemPrefab);
            var newId = _world.AllocateItemId();
            var worldItemState = new ItemState(newId, inventoryItems.DefId, inventoryItems.Count);
            worldItem.Init(worldItemState, def);
            worldItem.SetPosition(position);
            _world.WorldItems.Add(worldItemState.Id, worldItem);
        }
    }
}