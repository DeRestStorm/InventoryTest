using System.Collections.Generic;
using SuperGame.ECS;
using UnityEngine;

namespace SuperGame
{
    [RequireComponent(typeof(SphereCollider))]
    public class ItemPickupRange : MonoBehaviour
    {
        [SerializeField] SphereCollider _sphereCollider;

        readonly List<WorldItem> _itemsInRange = new List<WorldItem>();

        public IReadOnlyList<WorldItem> ItemsInRange => _itemsInRange;

        public void SetRange(float range)
        {
            if (_sphereCollider is null)
                return;
            _sphereCollider.radius = range;
        }

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
