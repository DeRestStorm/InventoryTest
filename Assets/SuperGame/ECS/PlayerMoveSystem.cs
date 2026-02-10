using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SuperGame.ECS
{
    public partial struct PlayerMoveSystem : ISystem
    {
        const float MoveSpeed = 5f;

        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.TryGetSingleton<PlayerMoveInput>(out var moveInput) is false)
                return;

            var dt = SystemAPI.Time.DeltaTime;
            var move = moveInput.Value * (MoveSpeed * dt);

            foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Player>())
            {
                var pos = transform.ValueRO.Position;
                pos.x += move.x;
                pos.z += move.y;
                transform.ValueRW.Position = pos;
            }
        }
    }
}
