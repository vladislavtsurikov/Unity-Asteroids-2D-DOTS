using Unity.Entities;

namespace VladislavTsurikov.EntitySpawning.Runtime
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming,
        WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming)]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class SpawnerSystemGroup : ComponentSystemGroup
    {
    }
}