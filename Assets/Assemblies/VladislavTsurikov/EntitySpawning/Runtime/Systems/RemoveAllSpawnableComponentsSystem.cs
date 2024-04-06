using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using VladislavTsurikov.EntitySpawning.Runtime.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct RemoveAllSpawnableComponentsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Spawnable>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            
            foreach (var (_, entity) in 
                     SystemAPI.Query<RefRO<Spawnable>>().WithEntityAccess())
            {
                entityCommandBuffer.RemoveComponent<Spawnable>(entity);
            }
            
            entityCommandBuffer.Playback(state.EntityManager);
            entityCommandBuffer.Dispose();
        }
    }
}