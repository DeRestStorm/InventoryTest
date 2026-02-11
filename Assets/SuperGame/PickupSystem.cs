using Definitions;
using SuperGame.ECS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace SuperGame
{
    public class PickupSystem : MonoBehaviour
    {
        [SerializeField] private ItemPickupRange _pickupRange;
        [SerializeField] private Camera _camera;
        [SerializeField] private UIDocument _takeUI;

        private VisualElement _takeContainer;
        private Label _takeText;
        private Commands _commands;

        public void Init(Commands commands)
        {
            _commands = commands;
        }

        private void Start()
        {
            var root = _takeUI.rootVisualElement;
            _takeContainer = root.Q<VisualElement>("TakeItemContainer");
            _takeText = root.Q<Label>("TakeText");
            _takeContainer.style.display = DisplayStyle.None;
        }

        private void Update()
        {
            var items = _pickupRange.ItemsInRange;
            if (items.Count == 0)
            {
                if(_takeContainer.style.display!=  DisplayStyle.None)
                    _takeContainer.style.display = DisplayStyle.None;
                return;
            }

            var cameraTransform = _camera.transform;
            var cameraDir = cameraTransform.forward;
            var cameraPos = cameraTransform.position;
            WorldItem closestItem = null;
            float maxDot = float.MinValue;

            foreach (var worldItem in items)
            {
                if (worldItem == null)
                    continue;
                var dirToItem = (worldItem.transform.position - cameraPos).normalized;
                // Вычесляем скалярное произведение для оперделения пложения относитель камеры
                // ближе к 1 - ближе к крсору (направлению камеры)
                // 0 это уже сбоку от камеры
                // -1 позади камеры
                var dot = Vector3.Dot(dirToItem, cameraDir); 
                if (dot > maxDot)
                {
                    closestItem = worldItem;
                    maxDot = dot;
                }
            }

            if (closestItem == null)
            {
                _takeContainer.style.display = DisplayStyle.None;
                return;
            }

            _takeContainer.style.display = DisplayStyle.Flex;

            var panelPos = RuntimePanelUtils.CameraTransformWorldToPanel(
                _takeText.panel, closestItem.transform.position, _camera);
            _takeText.style.left = panelPos.x - _takeText.resolvedStyle.width / 2f;
            _takeText.style.top = panelPos.y - _takeText.resolvedStyle.height;

            if (Keyboard.current is not null && Keyboard.current.eKey.wasPressedThisFrame)
                TryPickup(closestItem);
        }

        private void TryPickup(WorldItem worldItem)
        {
            var command = new PickupItemCommand(worldItem.ItemState.Id);
            _commands.PickupItem(command);
        }
    }
}
