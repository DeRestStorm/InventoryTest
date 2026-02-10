using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SuperGame.ECS
{
    public partial struct ItemSpinSystem : ISystem
    {
        const float DegreesPerSecond = 90f;

        public void OnUpdate(ref SystemState state)
        {
            var dt = SystemAPI.Time.DeltaTime;
            var angle = math.radians(DegreesPerSecond * dt);

            foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Item>())
                transform.ValueRW = transform.ValueRO.RotateY(angle);
        }
    }
}
