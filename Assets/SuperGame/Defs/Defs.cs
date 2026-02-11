using System.Collections.Generic;

namespace Definitions
{
    public record DefData
    {
        public Dictionary<ItemDefId, ItemDef> Items { get; set; }
        public int MaxInventorySize { get; set; } = 12;
    }

    public record ItemDef(string Name, string Color, bool Stackable, int MaxStack = 1)
    {
        public string Name { get; } = Name;
        public string Color { get; } = Color;
        public bool Stackable { get; } = Stackable;
        public int MaxStack { get; } = Stackable ? MaxStack : 1;
    }

    public record ItemDefId(string Id)
    {
        public string Id { get; } = Id;
    }
}
