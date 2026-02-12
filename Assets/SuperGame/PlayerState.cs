using System;
using System.Collections.Generic;
using System.Linq;
using Definitions;

namespace SuperGame
{
    public class PlayerState
    {
        public InventoryState Inventory = new();
    }

    public class InventoryState
    {
        private int _nextItemId = 1;

        public event Action OnChanged;

        public Dictionary<int, short> Slots = new();
        public Dictionary<int, ItemState> Items = new();

        public int AllocateItemId()
        {
            if (Items.Count > 0)
            {
                int max = Items.Keys.Max();
                if (_nextItemId <= max)
                    _nextItemId = max + 1;
            }
            return _nextItemId++;
        }

        public void NotifyChanged() => OnChanged?.Invoke();
    }

    public class ItemState
    {
        public readonly int Id;
        public readonly ItemDefId DefId;
        public int Count;

        public ItemState(int id, ItemDefId defId, int count)
        {
            Id = id;
            DefId = defId;
            Count = count;
        }
    }
}