using System;
using UnityEngine.Serialization;

namespace Definitions
{
    [Serializable]
    public class DefDataDto
    {
        public ItemDefDto[] Items;
        public int MaxInventorySize = 12;
    }

    [Serializable]
    public class ItemDefDto
    {
        public string Id;
        public string Name;
        public string Color;
        public bool Stackable;
        public int MaxStack = 1;

        public void Deconstruct(out string id, out string name, out string color, out bool stackable, out int maxStack)
        {
            id = Id;
            name = Name;
            color = Color;
            stackable = Stackable;
            maxStack = MaxStack;
        }
    }
}
