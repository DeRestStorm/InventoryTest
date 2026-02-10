using System;
using Definitions;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SuperGame.ECS
{
    public class ItemAuthoring : MonoBehaviour
    {
        public string DefId;
        public MeshRenderer[] Renderers;

        void OnValidate()
        {
            if (Renderers is null || Renderers.Length == 0)
                Renderers = GetComponentsInChildren<MeshRenderer>(true);
        }

        class Baker : Baker<ItemAuthoring>
        {
            public override void Bake(ItemAuthoring authoring)
            {
                if (authoring.Renderers is null || authoring.Renderers.Length == 0)
                    authoring.Renderers = authoring.GetComponentsInChildren<MeshRenderer>(true);

                if (Enum.TryParse<ItemDefId>(authoring.DefId, out var itemDefId) is false)
                {
                    Debug.LogError($"Invalid itemId {authoring.DefId}");
                    return;
                }

                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new Item { DefId = itemDefId });
                AddComponentObject(entity, new RendererData { Renderers = authoring.Renderers });
                AddComponent(entity, new NotInited());
            }
        }
    }
}
