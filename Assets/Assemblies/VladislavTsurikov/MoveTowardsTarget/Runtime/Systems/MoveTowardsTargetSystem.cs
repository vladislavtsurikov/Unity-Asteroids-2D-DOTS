using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.MoveTowardsTarget.Runtime.Components;

namespace VladislavTsurikov.MoveTowardsTarget.Runtime.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct MoveTowardsTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TargetComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (target, localTransform, entity) 
                     in SystemAPI.Query<RefRO<TargetComponent>, RefRW<LocalTransform>>().WithEntityAccess())
            {
                if (state.EntityManager.Exists(target.ValueRO.TargetEntity))
                {
                    float speed = target.ValueRO.Speed;
                    float3 targetPosition = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.TargetEntity).Position;
                    float3 direction = math.normalize(targetPosition - localTransform.ValueRW.Position);
                    localTransform.ValueRW.Position += direction * speed * SystemAPI.Time.DeltaTime;
                }
                else
                {
                    entityCommandBuffer.RemoveComponent<TargetComponent>(entity);
                }
            }
        }
    }
}