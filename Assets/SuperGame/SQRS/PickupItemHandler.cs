using System.Collections.Generic;
using Definitions;

namespace SuperGame
{
    public class PickupItemHandler
    {
        private readonly DefData _defs;
        private readonly InventoryState _inventory;
        private readonly WorldApi _worldApi;

        public PickupItemHandler(DefData defs, InventoryState inventory, WorldApi worldApi)
        {
            _defs = defs;
            _inventory = inventory;
            _worldApi = worldApi;
        }

        public void Handle(PickupItemCommand command)
        {
            if (_worldApi.TryGetItem(command.ItemId, out var itemState) is false) return;
            var defId = itemState.DefId;
            int remaining = itemState.Count;

            if (_defs.Items.TryGetValue(defId, out var def) && def.Stackable)
            {
                foreach (var (_, state) in _inventory.Items)
                {
                    if (state.DefId == defId && state.Count < def.MaxStack)
                    {
                        int canAdd = def.MaxStack - state.Count;
                        int toAdd = remaining < canAdd ? remaining : canAdd;
                        state.Count += toAdd;
                        remaining -= toAdd;
                        if (remaining <= 0) break;
                    }
                }
            }

            while (remaining > 0)
            {
                short freeSlot = FindFreeSlot();
                if (freeSlot == -1) break;

                int stackSize = (_defs.Items.TryGetValue(defId, out var d) && d.Stackable)
                    ? (remaining < d.MaxStack ? remaining : d.MaxStack)
                    : remaining;

                int itemId = _inventory.AllocateItemId();
                _inventory.Items[itemId] = new ItemState(itemId, defId, stackSize);
                _inventory.Slots[itemId] = freeSlot;
                remaining -= stackSize;
            }

            int pickedUp = itemState.Count - remaining;
            if (pickedUp <= 0) return;

            _inventory.NotifyChanged();

            if (remaining > 0)
                _worldApi.UpdateItemCount(command.ItemId, remaining);
            else
                _worldApi.DestroyItem(command.ItemId);
        }

        private short FindFreeSlot()
        {
            var occupiedSlots = new HashSet<short>(_inventory.Slots.Values);
            for (short i = 0; i < _defs.MaxInventorySize; i++)
            {
                if (occupiedSlots.Contains(i) is false)
                    return i;
            }
            return -1;
        }
    }
}
