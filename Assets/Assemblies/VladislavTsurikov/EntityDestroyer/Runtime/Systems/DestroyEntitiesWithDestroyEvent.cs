using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.EntityDestroyer.Runtime.Components;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Systems
{
    [UpdateInGroup(typeof(DestroyEntityGroupSystem), OrderLast = true)]
    public partial struct DestroyEntitiesWithDestroyEvent : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DestroyEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer commandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (_, entity) in SystemAPI.Query<RefRO<DestroyEvent>>().WithEntityAccess())
            {
                commandBuffer.DestroyEntity(entity);
            }
        }
    }
}