using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using VladislavTsurikov.EntityDestroyer.Runtime.Components;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.DestroyOnCollision.Components;
using VladislavTsurikov.Utility.Runtime;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.DestroyOnCollision.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct DestroyOnCollisionSystem : ISystem
    {
        private ComponentLookup<DestroyOnCollisionComponent> _destroyOnCollisionGroup;
        private ComponentLookup<EntityLayer.Runtime.Components.EntityLayer> _entityLayerGroup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DestroyOnCollisionComponent>();
            
            _destroyOnCollisionGroup = state.GetComponentLookup<DestroyOnCollisionComponent>();
            _entityLayerGroup = state.GetComponentLookup<EntityLayer.Runtime.Components.EntityLayer>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _destroyOnCollisionGroup.Update(ref state);
            _entityLayerGroup.Update(ref state);
            
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer commandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            state.Dependency = new TriggerJob
            {
                DestroyOnCollisionGroup = _destroyOnCollisionGroup,
                EntityLayerGroup = _entityLayerGroup,
                CommandBuffer = commandBuffer
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        [BurstCompile]
        private struct TriggerJob : ITriggerEventsJob
        {
            [ReadOnly] 
            public ComponentLookup<DestroyOnCollisionComponent> DestroyOnCollisionGroup;
            [ReadOnly] 
            public ComponentLookup<EntityLayer.Runtime.Components.EntityLayer> EntityLayerGroup;
            public EntityCommandBuffer CommandBuffer;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                DestroyIfPossible(triggerEvent.EntityA, triggerEvent.EntityB);
                DestroyIfPossible(triggerEvent.EntityB, triggerEvent.EntityA);
            }

            private void DestroyIfPossible(Entity destroyableEntity, Entity triggerEntity)
            {
                if (ShouldDestroy(destroyableEntity, triggerEntity))
                {
                    CommandBuffer.AddComponent<DestroyEvent>(destroyableEntity);
                }
            }
            
            private bool ShouldDestroy(Entity destroyableEntity, Entity triggerEntity)
            {
                if (!DestroyOnCollisionGroup.HasComponent(destroyableEntity))
                {
                    return false;
                }
                
                if (!EntityLayerGroup.HasComponent(triggerEntity))
                {
                    return false;
                }
                
                var destroyOnCollisionTag = DestroyOnCollisionGroup[destroyableEntity];
                var collisionFilter = destroyOnCollisionTag.CollisionFilter;

                int triggerLayerMask = EntityLayerGroup[triggerEntity].LayerMask;

                return collisionFilter.IsCollisionEnabled(triggerLayerMask);
            }
        }
    }
}