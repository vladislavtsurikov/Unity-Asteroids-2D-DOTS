using Unity.Entities;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.Restart
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming,
        WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming)]
    [UpdateInGroup(typeof(ApplicationStateSystemGroup), OrderFirst = true)]
    public partial class RestartSystemGroup : ComponentSystemGroup
    {
    }
}