using Definitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace SuperGame
{
    public class InventorySlotView
    {
        public int SlotIndex { get; }
        public VisualElement Root { get; }
        public ItemState Item => _item;

        VisualElement _icon;
        Label _name;
        Label _count;
        ItemState _item;
        readonly DefData _defData;

        public InventorySlotView(VisualElement root, int slotIndex, DefData defData)
        {
            Root = root;
            SlotIndex = slotIndex;
            _defData = defData;
            _icon = root.Q<VisualElement>("Icon");
            _name = root.Q<Label>("Name");
            _count = root.Q<Label>("Count");
            Clear();
        }

        public void SetItem(ItemState item)
        {
            _item = item;
            _icon.EnableInClassList("inventory-slot__item", item is not null);

            if (item is null)
            {
                Clear();
                return;
            }

            if (_defData.Items is null || _defData.Items.TryGetValue(item.DefId, out var def) is false)
            {
                Clear();
                return;
            }

            if (ColorUtility.TryParseHtmlString(def.Color, out var color))
                _icon.style.unityBackgroundImageTintColor = new StyleColor(color);
            else
                _icon.style.unityBackgroundImageTintColor = new StyleColor(Color.white);

            _name.text = def.Name;
            _name.style.display = DisplayStyle.Flex;

            _count.text = item.Count.ToString();
            _count.style.display = def.Stackable || item.Count > 1 ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void Clear()
        {
            _item = null;
            _icon.EnableInClassList("inventory-slot__item", false);
            _icon.style.unityBackgroundImageTintColor = new StyleColor(Color.white);
            _name.style.display = DisplayStyle.None;
            _count.style.display = DisplayStyle.None;
        }

        public void SetHighlight(bool on)
        {
            Root.EnableInClassList("inventory-slot--highlight", on);
        }
    }
}
