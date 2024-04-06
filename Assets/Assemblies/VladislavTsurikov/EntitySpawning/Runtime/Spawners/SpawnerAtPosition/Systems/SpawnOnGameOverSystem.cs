using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Systems
{
    [UpdateInGroup(typeof(GameOverSystemGroup))]
    public partial struct SpawnOnGameOverSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameOverEvent>();
            state.RequireForUpdate<SpawnerAtPositionComponent>();
            state.RequireForUpdate<SpawnOnRestartEventComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawnerComponent, _)
                     in SystemAPI.Query<RefRW<SpawnerAtPositionComponent>, SpawnOnGameOverEventComponent>())
            {
                spawnerComponent.ValueRW.Spawn = true;
            }
        }
    }
}