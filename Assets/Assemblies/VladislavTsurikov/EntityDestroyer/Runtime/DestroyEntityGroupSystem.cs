using Unity.Entities;

namespace VladislavTsurikov.EntityDestroyer.Runtime
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming,
        WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming)]
    [UpdateInGroup(typeof(LateSimulationSystemGroup), OrderFirst = true)]
    public partial class DestroyEntityGroupSystem : ComponentSystemGroup
    {
    }
}