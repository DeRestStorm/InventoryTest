using System.Collections.Generic;

namespace Definitions
{
    
    public static class Defs
    {
        private static DefData _defData;
        public static void Init(DefData defData)
        {
            _defData = defData;
        }


        public static IReadOnlyDictionary<ItemDefId, ItemDef> Items => _defData?.Items;
        public static int MaxInventorySize => _defData.MaxInventorySize;

    }
    
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

    public enum ItemDefId
    {
        Cat,
        Poop
    }
}
