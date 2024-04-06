using Unity.Entities;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial struct SpawnerGameObjectSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnerAtPositionComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawnComponent, spawnPrefab, entity) in SystemAPI.Query<RefRW<SpawnerAtPositionComponent>, SpawnPrefab>().WithEntityAccess())
            {
                if (spawnComponent.ValueRW.Spawn)
                {
                    if (state.EntityManager.HasComponent<SpawnAtPositionComponent>(entity))
                    {
                        SpawnAtPositionComponent spawnAtPositionComponent = state.EntityManager.GetComponentData<SpawnAtPositionComponent>(entity);

                        spawnPrefab.SpawnAtPosition(spawnAtPositionComponent.SpawnPosition);
                    }
                    else
                    {
                        spawnPrefab.Spawn();
                    }
                    
                    spawnComponent.ValueRW.Spawn = false;
                }
            }
        }
    }
}