using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using VladislavTsurikov.EntityDestroyer.Runtime.Components;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct DamageCollisionSystem : ISystem
    {
        private ComponentLookup<DamagingComponent> _damagingComponentGroup;
        private ComponentLookup<HealthComponent> _healthComponentGroup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DamagingComponent>();
            state.RequireForUpdate<HealthComponent>();
            
            _damagingComponentGroup = state.GetComponentLookup<DamagingComponent>();
            _healthComponentGroup = state.GetComponentLookup<HealthComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _damagingComponentGroup.Update(ref state);
            _healthComponentGroup.Update(ref state);
            
            var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            state.Dependency = new DamageCollisionJob
            {
                DamagingGroup = _damagingComponentGroup,
                HealthGroup = _healthComponentGroup,
                CommandBuffer = entityCommandBuffer
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
        
        [BurstCompile]
        public struct DamageCollisionJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentLookup<DamagingComponent> DamagingGroup;
            public ComponentLookup<HealthComponent> HealthGroup;
            public EntityCommandBuffer CommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                DamageIfPossible(triggerEvent.EntityA, triggerEvent.EntityB);
                DamageIfPossible(triggerEvent.EntityB, triggerEvent.EntityA);
            }

            private void DamageIfPossible(Entity damageTakerEntity, Entity damagingEntity)
            {
                if (!IsPossibleDamage(damageTakerEntity, damagingEntity))
                {
                    return;
                }
                
                Damage(DamagingGroup[damagingEntity].DamageValue, damageTakerEntity);
                        
                if (DamagingGroup[damagingEntity].DestroyDamagingEntity)
                {
                    CommandBuffer.AddComponent<DestroyEvent>(damagingEntity);
                }
            }

            private bool IsPossibleDamage(Entity damageTakerEntity, Entity damagingEntity)
            {
                return HealthGroup.HasComponent(damageTakerEntity) && DamagingGroup.HasComponent(damagingEntity);
            }

            private void Damage(float damage, Entity entity)
            {
                var health = HealthGroup[entity];
                
                health.Value -= damage;

                if (health.Value <= 0)
                {
                    CommandBuffer.AddComponent<DestroyEvent>(entity);
                    HealthGroup[entity] = health;
                }
                else
                {
                    HealthGroup[entity] = health;
                }
            }
        }
    }
}