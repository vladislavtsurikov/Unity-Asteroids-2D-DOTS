using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.EntityDestroyer.Runtime.Components;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Systems
{
    [UpdateInGroup(typeof(DestroyEntityGroupSystem), OrderFirst = true)]
    public partial struct DestroyEntitiesWithoutHealthSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<HealthComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer commandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (healthComponent, entity) in SystemAPI
                         .Query<RefRO<HealthComponent>>().WithEntityAccess())
            {
                if (healthComponent.ValueRO.Value <= 0)
                {
                    commandBuffer.AddComponent<DestroyEvent>(entity);
                }
            }
        }
    }
}