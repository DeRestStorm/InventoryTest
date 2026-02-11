using System;
using Definitions;
using UnityEngine;

namespace SuperGame.ECS
{
    public class WorldItem : MonoBehaviour
    {
        public ItemState ItemState { get; private set; }
        public string DefId;
        public int Count;
        public MeshRenderer[] Renderers;

        public void Init(ItemState state, ItemDef def)
        {
            ItemState = state;
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

        private void OnValidate()
        {
            if (Renderers is null || Renderers.Length == 0)
                Renderers = GetComponentsInChildren<MeshRenderer>(true);
        }
    }
}
