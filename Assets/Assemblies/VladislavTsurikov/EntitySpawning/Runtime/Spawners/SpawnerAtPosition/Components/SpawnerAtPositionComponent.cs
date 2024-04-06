using Unity.Entities;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components
{
    public struct SpawnerAtPositionComponent : IComponentData
    {
        public bool Spawn;
    }
}