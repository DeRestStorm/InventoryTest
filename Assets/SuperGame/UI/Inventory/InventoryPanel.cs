using System;
using System.Collections.Generic;
using Definitions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace SuperGame
{
    public class InventoryPanel : MonoBehaviour
    {
        public event Action<bool> OnToggled;

        [SerializeField] UIDocument _document;
        [SerializeField] VisualTreeAsset _slotTemplate;

        VisualElement _root;
        ScrollView _scrollView;
        bool _visible;

        InventoryState _inventory;
        Action<MoveItemCommand> _onMoveCommand;

        readonly List<InventorySlotView> _slots = new();

        InventorySlotView _dragSource;
        VisualElement _dragGhost;
        bool _isDragging;

        public void Init(InventoryState inventory, Action<MoveItemCommand> onMoveCommand)
        {
            _inventory = inventory;
            _onMoveCommand = onMoveCommand;
            _inventory.OnChanged += RefreshSlots;
        }

        void Awake()
        {
            if (_document is null)
                _document = GetComponent<UIDocument>();
        }

        void Start()
        {
            _root = _document.rootVisualElement;
            _root.style.display = DisplayStyle.None;
            _visible = false;

            _scrollView = _root.Q<ScrollView>("InventoryScrollView");

            CreateSlots();

            _root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        void CreateSlots()
        {
            var container = _scrollView.contentContainer;
            container.Clear();
            container.AddToClassList("inventory-grid");
            _slots.Clear();

            int maxSlots = Defs.MaxInventorySize;

            for (int i = 0; i < maxSlots; i++)
            {
                var element = _slotTemplate.Instantiate();
                var slotView = new InventorySlotView(element.Q("Slot"), i);

                slotView.Root.RegisterCallback<PointerDownEvent>(evt => OnSlotPointerDown(evt, slotView));

                container.Add(element);
                _slots.Add(slotView);
            }
        }

        public void RefreshSlots()
        {
            foreach (var slot in _slots)
                slot.Clear();

            if (_inventory is null)
                return;

            foreach (var kvp in _inventory.Slots)
            {
                int itemId = kvp.Key;
                short position = kvp.Value;

                if (position < 0 || position >= _slots.Count)
                    continue;

                if (_inventory.Items.TryGetValue(itemId, out var item) is false)
                    continue;

                _slots[position].SetItem(item);
            }
        }

        void OnSlotPointerDown(PointerDownEvent evt, InventorySlotView slot)
        {
            if (evt.button != 0 || slot.Item is null)
                return;

            _dragSource = slot;
            _isDragging = true;

            _dragGhost = new VisualElement();
            _dragGhost.AddToClassList("drag-ghost");
            _dragGhost.pickingMode = PickingMode.Ignore;

            if (Defs.Items.TryGetValue(slot.Item.DefId, out var def)
                && ColorUtility.TryParseHtmlString(def.Color, out var color))
            {
                _dragGhost.style.backgroundColor = new StyleColor(color);
            }

            _root.Add(_dragGhost);
            UpdateGhostPosition(evt.position);

            slot.Root.style.opacity = 0.3f;
            evt.StopPropagation();
        }

        void OnPointerMove(PointerMoveEvent evt)
        {
            if (_isDragging is false || _dragGhost is null)
                return;

            UpdateGhostPosition(evt.position);

            ClearHighlights();
            var target = FindSlotUnderPointer(evt.position);
            if (target is not null && target != _dragSource)
                target.SetHighlight(true);
        }

        void OnPointerUp(PointerUpEvent evt)
        {
            if (_isDragging is false)
                return;

            ClearHighlights();
            var target = FindSlotUnderPointer(evt.position);

            if (target is not null && _dragSource is not null && target != _dragSource)
            {
                var command = new MoveItemCommand(_dragSource.Item.Id, (short)target.SlotIndex);
                _onMoveCommand?.Invoke(command);
            }

            EndDrag();
        }

        void UpdateGhostPosition(Vector3 pointerPosition)
        {
            var local = _root.WorldToLocal(new Vector2(pointerPosition.x, pointerPosition.y));
            _dragGhost.style.left = local.x - 35;
            _dragGhost.style.top = local.y - 35;
        }

        void EndDrag()
        {
            if (_dragSource is not null)
                _dragSource.Root.style.opacity = 1f;

            if (_dragGhost is not null)
            {
                _dragGhost.RemoveFromHierarchy();
                _dragGhost = null;
            }

            _dragSource = null;
            _isDragging = false;
        }

        void ClearHighlights()
        {
            foreach (var slot in _slots)
                slot.SetHighlight(false);
        }

        InventorySlotView FindSlotUnderPointer(Vector2 position)
        {
            var picked = _root.panel.Pick(position);
            if (picked is null)
                return null;

            foreach (var slot in _slots)
            {
                if (IsChildOf(picked, slot.Root))
                    return slot;
            }

            return null;
        }

        static bool IsChildOf(VisualElement element, VisualElement parent)
        {
            var current = element;
            while (current is not null)
            {
                if (current == parent)
                    return true;
                current = current.parent;
            }
            return false;
        }

        void OnDestroy()
        {
            if (_inventory is not null)
                _inventory.OnChanged -= RefreshSlots;
        }

        void Update()
        {
            if (Keyboard.current is null)
                return;
            if (Keyboard.current.tabKey.wasPressedThisFrame)
                Toggle();
        }

        void Toggle()
        {
            _visible = _visible is false;
            _root.style.display = _visible ? DisplayStyle.Flex : DisplayStyle.None;

            if (_visible)
                RefreshSlots();

            OnToggled?.Invoke(_visible);
        }
    }
}
