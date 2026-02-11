using System.Collections.Generic;
using System.Linq;
using Definitions;

namespace SuperGame
{
    public class PickupItemHandler
    {
        private readonly DefData _defs;
        private readonly InventoryState _inventory;
        private readonly WorldApi _worldApi;
        private int _nextItemId = 1;

        public PickupItemHandler(DefData defs, InventoryState inventory, WorldApi worldApi)
        {
            _defs = defs;
            _inventory = inventory;
            _worldApi = worldApi;

            if (inventory.Items.Count > 0)
                _nextItemId = inventory.Items.Keys.Max() + 1;
        }

        public void Handle(PickupItemCommand command)
        {
            if (_worldApi.TryGetItem(command.ItemId, out var itemState) is false) return;
            var defId = itemState.DefId; 

            if (_defs.Items.TryGetValue(defId, out var def) && def.Stackable)
            {
                foreach (var (_, state) in _inventory.Items)
                {
                    if (state.DefId == defId && state.Count < def.MaxStack)
                    {
                        state.Count++;
                        _inventory.NotifyChanged();
                        _worldApi.DestroyItem(command.ItemId);
                        return;
                    }
                }
            }

            var occupiedSlots = new HashSet<short>(_inventory.Slots.Values);
            short freeSlot = -1;
            for (short i = 0; i < _defs.MaxInventorySize; i++)
            {
                if (occupiedSlots.Contains(i) is false)
                {
                    freeSlot = i;
                    break;
                }
            }

            if (freeSlot == -1) return;

            int itemId = _nextItemId++;
            _inventory.Items[itemId] = new ItemState(itemId, defId, 1);
            _inventory.Slots[itemId] = freeSlot;
            _inventory.NotifyChanged();
            _worldApi.DestroyItem(command.ItemId);
        }
    }
}
