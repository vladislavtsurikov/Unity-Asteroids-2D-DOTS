using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Systems
{
    [UpdateInGroup(typeof(RestartSystemGroup))]
    public partial struct SpawnOnRestartSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RestartEvent>();
            state.RequireForUpdate<SpawnerAtPositionComponent>();
            state.RequireForUpdate<SpawnOnRestartEventComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawnerComponent, _)
                     in SystemAPI.Query<RefRW<SpawnerAtPositionComponent>, SpawnOnRestartEventComponent>())
            {
                spawnerComponent.ValueRW.Spawn = true;
            }
        }
    }
}