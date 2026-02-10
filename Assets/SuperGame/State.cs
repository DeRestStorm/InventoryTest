using System.Collections.Generic;
using Definitions;

namespace SuperGame
{
    public class State
    {
        public InventoryState Inventory = new();
    }

    public class InventoryState
    {
        public Dictionary<int, short> Slots = new();
        public Dictionary<int, ItemState> Items = new();
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