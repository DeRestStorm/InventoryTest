using System;
using System.Collections.Generic;
using Definitions;
using UnityEngine;

namespace SuperGame
{
    public static class InitDefs
    {
        public static DefData LoadFromJson()
        {
            TextAsset asset = Resources.Load<TextAsset>("DefData");
            if (asset is null)
                return new DefData { Items = new Dictionary<ItemDefId, ItemDef>() };

            DefDataDto dto = JsonUtility.FromJson<DefDataDto>(asset.text);

            var items = new Dictionary<ItemDefId, ItemDef>();
            if (dto.Items is not null)
            {
                foreach (ItemDefDto entry in dto.Items)
                {
                    var (stringId, name, color, stackable, maxStack) = entry;
                    var itemDefId = new ItemDefId(stringId);
                    items[itemDefId] = new ItemDef(name, color, stackable, maxStack);
                }
            }

            return new DefData
            {
                Items = items,
                MaxInventorySize = dto.MaxInventorySize
            };
        }
    }
}
