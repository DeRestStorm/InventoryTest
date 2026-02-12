using Definitions;
using UnityEngine;

namespace SuperGame.ECS
{
    public class WorldItem : MonoBehaviour
    {
        [field: SerializeField] public string DefId { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
        [field: SerializeField] public MeshRenderer[] Renderers { get; private set; }
        [SerializeField] private Rigidbody _rigidbody;
        public ItemState ItemState { get; private set; }

        public void Init(ItemState state, ItemDef def)
        {
            ItemState = state;
            Count = state.Count;
            SetMeshColor(def);
        }

        private void SetMeshColor(ItemDef def)
        {
            if (ColorUtility.TryParseHtmlString(def.Color, out Color c) is false)
                c = Color.white;
            if (Renderers is null || Renderers.Length == 0)
                Renderers = GetComponentsInChildren<MeshRenderer>(true);
            foreach (var r in Renderers)
            {
                //Крашу VC, шейдера партиклов его поддерживают
                //Вообще я бы красил черел MeshRenderer.SetShaderUserValue, но он 6000.3+
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

        public void SetPosition(Vector3 position)
        {
            _rigidbody.position = position;
            transform.position = position;
        }

        private void OnValidate()
        {
            if (Renderers is null || Renderers.Length == 0)
                Renderers = GetComponentsInChildren<MeshRenderer>(true);
            if (_rigidbody is null)
                _rigidbody = GetComponent<Rigidbody>();
        }
    }
}
