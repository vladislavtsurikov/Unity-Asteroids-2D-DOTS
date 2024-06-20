using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Systems;
using VladislavTsurikov.EntityEffects.Effects.Split.Components;
using VladislavTsurikov.Weapons.Runtime.Bullet.Components;

namespace VladislavTsurikov.Weapons.Runtime.Bullet.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(DamageCollisionSystem))]
    public partial struct BulletCollisionSystem : ISystem
    {
        private ComponentLookup<BulletTag> _damagingLookup;
        private ComponentLookup<HealthComponent> _healthLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BulletTag>();
            state.RequireForUpdate<HealthComponent>();
            
            _damagingLookup = state.GetComponentLookup<BulletTag>();
            _healthLookup = state.GetComponentLookup<HealthComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _damagingLookup.Update(ref state);
            _healthLookup.Update(ref state);
            
            var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            state.Dependency = new WeaponCollisionJob
            {
                DamagingLookup = _damagingLookup,
                HealthLookup = _healthLookup,
                CommandBuffer = entityCommandBuffer
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
        
        [BurstCompile]
        struct WeaponCollisionJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentLookup<BulletTag> DamagingLookup;
            [ReadOnly] public ComponentLookup<HealthComponent> HealthLookup;
            public EntityCommandBuffer CommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                DestroyIfPossible(triggerEvent.EntityA, triggerEvent.EntityB);
                DestroyIfPossible(triggerEvent.EntityB, triggerEvent.EntityA);
            }

            private void DestroyIfPossible(Entity damageTakerEntity, Entity damagingEntity)
            {
                if (!IsPossibleDestroy(damageTakerEntity, damagingEntity))
                {
                    return;
                }

                if (HealthLookup[damageTakerEntity].Value == 0)
                {
                    CommandBuffer.AddComponent<SplitDamage>(damageTakerEntity);
                }
            }

            private bool IsPossibleDestroy(Entity damageTakerEntity, Entity damagingEntity)
            {
                return HealthLookup.HasComponent(damageTakerEntity) && DamagingLookup.HasComponent(damagingEntity);
            }
        }
    }
}