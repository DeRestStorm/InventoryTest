using System;
using Definitions;
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

        void Start()
        {
            if (Enum.TryParse<ItemDefId>(DefId, out var itemDefId) is false)
                return;
            var items = Defs.Items;
            if (items is null || items.TryGetValue(itemDefId, out ItemDef def) is false)
                return;
            if (ColorUtility.TryParseHtmlString(def.Color, out Color c) is false)
                c = Color.white;
            if (Renderers is null || Renderers.Length == 0)
                Renderers = GetComponentsInChildren<MeshRenderer>(true);
            foreach (var r in Renderers)
            {
                var filter = r.GetComponent<MeshFilter>();
                if (filter is null || filter.sharedMesh is null)
                    continue;
                var mesh = Instantiate(filter.sharedMesh);
                var colors = new Color[mesh.vertexCount];
                for (var i = 0; i < colors.Length; i++)
                    colors[i] = c;
                mesh.SetColors(colors);
                filter.sharedMesh = mesh;
            }
        }
    }
}
