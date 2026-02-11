using Definitions;

namespace SuperGame
{
    public class Commands
    {
        readonly MoveItemHandler _moveItemHandler;
        readonly PickupItemHandler _pickupItemHandler;

        public Commands(DefData defs, InventoryState inventory, WorldApi worldApi)
        {
            _moveItemHandler = new MoveItemHandler(defs, inventory);
            _pickupItemHandler = new PickupItemHandler(defs, inventory, worldApi);
        }

        public void MoveItem(MoveItemCommand command)
        {
            _moveItemHandler.Handle(command);
        }

        public void PickupItem(PickupItemCommand command)
        {
            _pickupItemHandler.Handle(command);
        }
    }
}
