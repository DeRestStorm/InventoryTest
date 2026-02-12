using Definitions;

namespace SuperGame
{
    public class Commands
    {
        readonly MoveItemHandler _moveItemHandler;
        readonly PickupItemHandler _pickupItemHandler;
        readonly DropItemHandler _dropItemHandler;

        public Commands(DefData defs, InventoryState inventory, WorldApi worldApi)
        {
            _moveItemHandler = new MoveItemHandler(defs, inventory);
            _pickupItemHandler = new PickupItemHandler(defs, inventory, worldApi);
            _dropItemHandler = new DropItemHandler(inventory, worldApi);
        }

        public void MoveItem(int itemId, short toPosition)
        {
            _moveItemHandler.Handle(new MoveItemCommand(itemId, toPosition));
        }

        public void PickupItem(int itemId)
        {
            _pickupItemHandler.Handle(new PickupItemCommand(itemId));
        }

        public void DropItem(int itemId)
        {
            _dropItemHandler.Handle(new DropItemCommand(itemId));
        }
    }
}
