using Unity.Entities;
using Unity.Mathematics;

namespace VladislavTsurikov.EntitySpawning.Runtime.Components
{
    public struct SpawnerRandom : IComponentData
    {
        public Random Value;
    }
}