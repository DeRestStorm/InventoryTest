using Definitions;

namespace SuperGame
{
    public class MoveItemHandler
    {
        private readonly DefData _defs;
        private readonly InventoryState _inventoryState;

        public MoveItemHandler(DefData defs, InventoryState inventoryState)
        {
            _defs = defs;
            _inventoryState = inventoryState;
        }

        public void Handle(MoveItemCommand command)
        {
            int itemId = command.ItemId;
            short toPosition = command.ToPosition;

            if (_inventoryState.Items.ContainsKey(itemId) is false)
                return;

            if (_inventoryState.Slots.TryGetValue(itemId, out short currentSlot) is false)
                return;

            if (currentSlot == toPosition)
                return;

            int otherItemId = -1;
            foreach (var kvp in _inventoryState.Slots)
            {
                if (kvp.Value == toPosition && kvp.Key != itemId)
                {
                    otherItemId = kvp.Key;
                    break;
                }
            }

            if (otherItemId >= 0)
                _inventoryState.Slots[otherItemId] = currentSlot;

            _inventoryState.Slots[itemId] = toPosition;
            _inventoryState.NotifyChanged();
        }
    }
}
