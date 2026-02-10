using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using SuperGame;

namespace SuperGame.ECS
{
    [RequireMatchingQueriesForUpdate]
    [UpdateBefore(typeof(PlayerMoveSystem))]
    public partial struct InputSyncSystem : ISystem
    {
        static GameInputSettings _cachedSettings;

        public void OnCreate(ref SystemState state)
        {
            _cachedSettings = Resources.Load<GameInputSettings>(GameInputSettings.ResourcePath);
        }

        public void OnUpdate(ref SystemState state)
        {
            var query = SystemAPI.QueryBuilder().WithAll<PlayerInputData, PlayerMoveInput>().Build();
            using var entities = query.ToEntityArray(Allocator.Temp);
            if (entities.Length == 0)
                return;

            var entity = entities[0];
            var data = state.EntityManager.GetComponentObject<PlayerInputData>(entity);
            var settings = data.Settings ?? _cachedSettings;
            if (settings is null)
                return;

            var moveAction = settings.MoveAction;
            if (moveAction is null)
                return;

            var v = moveAction.ReadValue<Vector2>();
            state.EntityManager.SetComponentData(entity, new PlayerMoveInput { Value = new float2(v.x, v.y) });
        }
    }
}
