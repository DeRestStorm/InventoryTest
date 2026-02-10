using Definitions;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SuperGame.ECS
{
    public partial struct ItemColorInitSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NotInited>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var items = Defs.Items;
            if (items is null)
                return;

            var entityManager = state.EntityManager;

            var query = SystemAPI.QueryBuilder().WithAll<NotInited, Item, RendererData>().Build();
            using var entities = query.ToEntityArray(Allocator.Temp);

            foreach (var entity in entities)
            {
                var item = entityManager.GetComponentData<Item>(entity);
                var rendererData = entityManager.GetComponentObject<RendererData>(entity);
                if (rendererData is null)
                    continue;

                var defId = item.DefId;
                if (items.TryGetValue(defId, out ItemDef def) is false)
                    continue;

                if (ColorUtility.TryParseHtmlString(def.Color, out Color c) is false)
                    c = Color.white;

                if (rendererData.Renderers is null)
                    continue;

                foreach (var renderer in rendererData.Renderers)
                {
                    if (renderer is null)
                        continue;
                    var meshFilter = renderer.GetComponent<MeshFilter>();
                    if (meshFilter is null)
                        continue;
                    var mesh = meshFilter.mesh;
                    var colors = new Color[mesh.vertexCount];
                    for (var i = 0; i < colors.Length; i++)
                        colors[i] = c;
                    mesh.SetColors(colors);
                }

                entityManager.RemoveComponent<NotInited>(entity);
            }
        }
    }
}