using Definitions;
using Unity.Entities;
using UnityEngine;

namespace SuperGame.ECS
{
    public struct Item : IComponentData
    {
        public ItemDefId DefId;
    }
}
