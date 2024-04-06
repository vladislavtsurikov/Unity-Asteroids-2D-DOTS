using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.Core.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Systems
{
    public partial struct SpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnerAtPositionComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            
            foreach (var (spawnComponent, prototypes, spawnerEntity) in SystemAPI.Query<RefRW<SpawnerAtPositionComponent>, DynamicBuffer<PrototypeComponent>>().WithEntityAccess())
            {
                if (prototypes.Length == 0)
                {
                    continue;
                }
                
                if (spawnComponent.ValueRW.Spawn)
                {
                    Entity newEntity = commandBuffer.Instantiate(prototypes[0].Prefab);

                    SetPositionIfNecessary(ref state, commandBuffer, spawnerEntity, newEntity);

                    spawnComponent.ValueRW.Spawn = false;
                }
            }
            
            commandBuffer.Playback(state.EntityManager);
            commandBuffer.Dispose();
        }

        private void SetPositionIfNecessary(ref SystemState state, EntityCommandBuffer commandBuffer, Entity spawnerEntity, Entity newEntity)
        {
            if (state.EntityManager.HasComponent<SpawnAtPositionComponent>(spawnerEntity))
            {
                SpawnAtPositionComponent spawnAtPositionComponent =
                    state.EntityManager.GetComponentData<SpawnAtPositionComponent>(spawnerEntity);
                
                LocalTransform localTransform = new LocalTransform
                {
                    Position = spawnAtPositionComponent.SpawnPosition,
                    Rotation = quaternion.identity,
                    Scale = 1f
                };

                commandBuffer.SetComponent(newEntity, localTransform);
            }
        }
    }
}