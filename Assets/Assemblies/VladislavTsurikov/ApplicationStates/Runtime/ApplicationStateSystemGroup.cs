using Unity.Entities;

namespace VladislavTsurikov.ApplicationStates.Runtime
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming,
        WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming)]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    public partial class ApplicationStateSystemGroup : ComponentSystemGroup
    {
    }
}