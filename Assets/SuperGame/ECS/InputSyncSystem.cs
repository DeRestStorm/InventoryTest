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
        static InputAction _moveAction;

        public void OnCreate(ref SystemState state)
        {
            var settings = Resources.Load<GameInputSettings>(GameInputSettings.ResourcePath);
            if (settings is null || settings.InputActions is null)
                return;
            var map = settings.InputActions.FindActionMap("Player");
            map.Enable();
            _moveAction = map.FindAction("Move");
        }

        public void OnUpdate(ref SystemState state)
        {
            if (_moveAction is null)
                return;
            var query = SystemAPI.QueryBuilder().WithAll<PlayerInputData, PlayerMoveInput>().Build();
            using var entities = query.ToEntityArray(Allocator.Temp);
            if (entities.Length == 0)
                return;

            var entity = entities[0];
            var v = _moveAction.ReadValue<Vector2>();
            state.EntityManager.SetComponentData(entity, new PlayerMoveInput { Value = new float2(v.x, v.y) });
        }
    }
}
