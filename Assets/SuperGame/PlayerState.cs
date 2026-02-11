using System;
using System.Collections.Generic;
using Definitions;

namespace SuperGame
{
    public class PlayerState
    {
        public InventoryState Inventory = new();
    }

    public class InventoryState
    {
        public event Action OnChanged;

        public Dictionary<int, short> Slots = new();
        public Dictionary<int, ItemState> Items = new();

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