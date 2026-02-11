using Definitions;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
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

            var query = SystemAPI.QueryBuilder().WithAll<NotInited, Item, RenderMeshArray>().Build();
            using var entities = query.ToEntityArray(Allocator.Temp);

            foreach (var entity in entities)
            {
                var item = entityManager.GetComponentData<Item>(entity);
                var rendererData = entityManager.GetSharedComponentManaged<RenderMeshArray>(entity);

                var defId = item.DefId;
                if (items.TryGetValue(defId, out ItemDef def) is false)
                    continue;

                if (ColorUtility.TryParseHtmlString(def.Color, out Color c) is false)
                    c = Color.white;

                var meshCount = rendererData.MeshReferences.Length;
                var meshes = new Mesh[meshCount];
                for (var i = 0; i < meshCount; i++)
                {
                    var source = rendererData.MeshReferences[i].Value;
                    var copy = Object.Instantiate(source);
                    var colors = new Color[copy.vertexCount];
                    for (var j = 0; j < colors.Length; j++)
                        colors[j] = c;
                    copy.SetColors(colors);
                    meshes[i] = copy;
                }

                var materials = new Material[rendererData.MaterialReferences.Length];
                for (var i = 0; i < materials.Length; i++)
                    materials[i] = rendererData.MaterialReferences[i].Value;

                entityManager.SetSharedComponentManaged(entity, new RenderMeshArray(materials, meshes));
                entityManager.RemoveComponent<NotInited>(entity);
            }
        }
    }
}