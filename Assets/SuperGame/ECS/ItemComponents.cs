using Definitions;
using Unity.Collections;
using Unity.Entities;
using SuperGame;
using UnityEngine;

namespace SuperGame.ECS
{
    public struct Item : IComponentData
    {
        public ItemDefId DefId;
    }

    public struct NotInited : IComponentData
    {
    }

    public class RendererData : IComponentData
    {
        public MeshRenderer[] Renderers;
    }

    public struct Player : IComponentData
    {
    }

    public struct PlayerMoveInput : IComponentData
    {
        public Unity.Mathematics.float2 Value;
    }

    public class PlayerInputData : IComponentData
    {
        public GameInputSettings Settings;
    }
}
