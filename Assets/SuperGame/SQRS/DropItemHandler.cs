using Definitions;

namespace SuperGame
{
    public class DropItemHandler
    {
        private readonly InventoryState _inventoryState;
        private readonly WorldApi _worldApi;

        public DropItemHandler(InventoryState inventoryState, WorldApi worldApi)
        {
            _inventoryState = inventoryState;
            _worldApi = worldApi;
        }

        public void Handle(DropItemCommand command)
        {
            int itemId = command.ItemId;
            if (_inventoryState.Items.Remove(itemId, out var state) is false)
                return;
    
            _inventoryState.Slots.Remove(itemId);
            _inventoryState.NotifyChanged();
            _worldApi.SpawnItem(state);
        }
    }
}
