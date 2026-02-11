using System.Collections.Generic;
using SuperGame.ECS;
using UnityEngine;

namespace SuperGame
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickupRange : MonoBehaviour
    {
        readonly List<WorldItem> _itemsInRange = new List<WorldItem>();

        public IReadOnlyList<WorldItem> ItemsInRange => _itemsInRange;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<WorldItem>(out var item) is false)
                return;
            if (_itemsInRange.Contains(item))
                return;
            _itemsInRange.Add(item);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<WorldItem>(out var item) is false)
                return;
            _itemsInRange.Remove(item);
        }
    }
}
