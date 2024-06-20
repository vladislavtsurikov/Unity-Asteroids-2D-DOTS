using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components;
using VladislavTsurikov.Weapons.Runtime.Bullet.Components;

namespace VladislavTsurikov.Weapons.Runtime.Bullet.Aspects
{
    public readonly partial struct BulletWeaponAspect : IAspect
    {
        private readonly RefRW<BulletWeaponComponent> _weapon;
        private readonly RefRO<LocalToWorld> _localToWorld;

        public void OnUpdate(float deltaTime)
        {
            _weapon.ValueRW.CooldownShotsTimer -= deltaTime;
        }

        public void ShootIfNecessary(EntityCommandBuffer commandBuffer)
        {
            if (_weapon.ValueRO.CooldownShotsTimer <= 0f)
            {
                Shoot(commandBuffer);
            }
        }

        private void Shoot(EntityCommandBuffer commandBuffer)
        {
            var entity = commandBuffer.Instantiate(_weapon.ValueRO.Prefab);

            LocalTransform localTransform = new LocalTransform
            {
                Position = _localToWorld.ValueRO.Position,
                Scale = 1,
                Rotation = _localToWorld.ValueRO.Rotation,
            };
            
            commandBuffer.SetComponent(entity, localTransform);
            commandBuffer.AddComponent<BulletTag>(entity);

            var playerForward = math.mul(_localToWorld.ValueRO.Rotation, new float3(0, 1, 0));
            
            commandBuffer.AddComponent<global::VladislavTsurikov.PhysicsVelocity.Runtime.Components.CustomPhysicsVelocity>(entity);
            commandBuffer.SetComponent(entity, new global::VladislavTsurikov.PhysicsVelocity.Runtime.Components.CustomPhysicsVelocity
            {
                Linear = playerForward * _weapon.ValueRW.Speed
            });
            
            commandBuffer.AddComponent(entity, new DamagingComponent
            {
                DamageValue = _weapon.ValueRO.Damage,
                DestroyDamagingEntity = true
            });

            ResetCooldownShotsTimer();
        }
        
        private void ResetCooldownShotsTimer()
        {
            _weapon.ValueRW.CooldownShotsTimer = _weapon.ValueRW.CooldownShots;
        }
    }
}