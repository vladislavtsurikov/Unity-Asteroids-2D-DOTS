using Unity.Entities;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.GameOver
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming,
        WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ThinClientSimulation | WorldSystemFilterFlags.Streaming)]
    [UpdateInGroup(typeof(ApplicationStateSystemGroup), OrderFirst = true)]
    public partial class GameOverSystemGroup : ComponentSystemGroup
    {
    }
}