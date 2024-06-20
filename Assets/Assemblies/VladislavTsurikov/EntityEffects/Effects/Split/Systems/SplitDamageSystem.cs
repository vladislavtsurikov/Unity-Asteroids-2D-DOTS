using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.Core.Runtime.Components;
using VladislavTsurikov.EntityDestroyer.Runtime.Components;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components;
using VladislavTsurikov.EntityEffects.Effects.Split.Components;
using VladislavTsurikov.PhysicsVelocity.Runtime.Components;
using Random = Unity.Mathematics.Random;

namespace VladislavTsurikov.EntityEffects.Effects.Split.Systems
{
    public partial struct SplitDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SplitEntity>();
            state.RequireForUpdate<SplitDamage>();
            state.RequireForUpdate<DestroyEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer.ParallelWriter commandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            SplitJob splitJob = new SplitJob
            {
                CommandBuffer = commandBuffer,
            };
            
            JobHandle jobHandle = splitJob.ScheduleParallel(state.Dependency);
            jobHandle.Complete();
        }
        
        [BurstCompile]
        private partial struct SplitJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            
            private void Execute([EntityIndexInQuery] int sortKey, Entity entity, in SplitEntity splitEntity, in SplitDamage splitDamage, 
                in DynamicBuffer<PrototypeComponent> prototypes, in LocalTransform localTransform, in DestroyEvent destroyEvent)
            {
                for (int i = 0; i < splitEntity.SplitCount; i++)
                {
                    Random random = Random.CreateFromIndex((uint)(i + entity.GetHashCode()));
                    
                    PrototypeComponent prototypeComponent = PrototypeComponent.GetRandomPrototype(prototypes, ref random);
                    
                    var instance = CommandBuffer.Instantiate(sortKey, prototypeComponent.Prefab);

                    CommandBuffer.SetComponent(sortKey, instance, new LocalTransform
                    {
                        Position = localTransform.Position,
                        Rotation = quaternion.RotateZ(random.NextFloat(0f, 360)),
                        Scale = splitEntity.SplitSize,
                    });
                    
                    float speed = splitEntity.SplitSpeed;
                    float2 direction = random.NextFloat2Direction();
                    
                    CommandBuffer.AddComponent(sortKey, instance, new CustomPhysicsVelocity { Linear = new float3(direction.x, direction.y, 0) * speed });
                    CommandBuffer.AddComponent(sortKey, instance, new HealthComponent { Value = splitEntity.Health });
                }
            }
        }
    }
}