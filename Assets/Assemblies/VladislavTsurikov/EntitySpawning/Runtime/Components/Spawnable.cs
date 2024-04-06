using Unity.Entities;

namespace VladislavTsurikov.EntitySpawning.Runtime.Components
{
    public struct Spawnable : IComponentData
    {
        public int SpawnerHashCode;

        public Spawnable(int spawnerHashCode)
        {
            SpawnerHashCode = spawnerHashCode;
        }
    }
}