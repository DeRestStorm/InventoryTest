using Unity.Entities;
using UnityEngine;

namespace SuperGame.ECS
{
    public class PlayerInputAuthoring : MonoBehaviour
    {
        public GameInputSettings Settings;

        class Baker : Baker<PlayerInputAuthoring>
        {
            public override void Bake(PlayerInputAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponentObject(entity, new PlayerInputData { Settings = authoring.Settings });
                AddComponent(entity, new PlayerMoveInput());
            }
        }
    }
}
