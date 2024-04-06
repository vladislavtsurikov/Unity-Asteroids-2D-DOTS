using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Systems
{
    [UpdateInGroup(typeof(RestartSystemGroup))]
    public partial struct RestartIntervalSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RestartEvent>();
            state.RequireForUpdate<IntervalSpawnerComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawnerComponent, _)
                     in SystemAPI.Query<RefRW<IntervalSpawnerComponent>>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState).WithEntityAccess())
            {
                spawnerComponent.ValueRW.Reset();
            }
        }
    }
}