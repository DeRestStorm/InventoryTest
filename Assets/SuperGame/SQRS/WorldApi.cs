using UnityEngine;

namespace SuperGame
{
    public class WorldApi // Закос на обращение к миру на сервере из домена
    {
        private World _world;

        public WorldApi(World world)
        {
            _world = world;
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

        public void DestroyItem(int itemId)
        {
            if(_world.WorldItems.TryGetValue(itemId, out var worldItem) is false)
                return;
            GameObject.Destroy(worldItem.gameObject);
            _world.WorldItems.Remove(itemId);
        }
    }
}