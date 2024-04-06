using Unity.Entities;

namespace VladislavTsurikov.EntitySpawning.Runtime
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming,
        WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming)]
    [UpdateInGroup(typeof(SpawnerSystemGroup), OrderLast = true)]
    public partial class FindSpawnableComponentSystemGroup : ComponentSystemGroup
    {
    }
}