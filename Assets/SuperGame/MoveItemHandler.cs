using System.Collections.Generic;
using System.Linq;
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

            short currentSlot = -1;
            _inventoryState.Slots.TryGetValue(itemId, out currentSlot);

            int otherItemId = _inventoryState.Slots
                .Where(x => x.Value == toPosition)
                .Select(x => x.Key)
                .FirstOrDefault();

            if (otherItemId != 0)
            {
                if (currentSlot == -1)
                {
                    var occupiedSlots = _inventoryState.Slots.Values.ToHashSet();
                    short freeSlot = 0;
                    while (occupiedSlots.Contains(freeSlot))
                        freeSlot++;
                    _inventoryState.Slots[otherItemId] = freeSlot;
                }
                else
                {
                    _inventoryState.Slots[otherItemId] = currentSlot;
                }
            }

            _inventoryState.Slots[itemId] = toPosition;
        }
    }
}
