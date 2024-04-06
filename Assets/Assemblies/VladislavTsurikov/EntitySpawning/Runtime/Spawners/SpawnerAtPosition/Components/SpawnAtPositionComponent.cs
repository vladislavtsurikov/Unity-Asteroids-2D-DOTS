using Unity.Entities;
using Unity.Mathematics;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components
{
    public struct SpawnAtPositionComponent : IComponentData
    {
        public float3 SpawnPosition;
    }
}