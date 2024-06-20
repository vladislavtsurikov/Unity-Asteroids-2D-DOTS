using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components;
using VladislavTsurikov.Weapons.Runtime.Laser.Components;

namespace VladislavTsurikov.Weapons.Runtime.Laser.Aspects
{
    public readonly partial struct LaserWeaponAspect : IAspect
    {
        private readonly RefRW<LaserWeaponComponent> _weapon;
        private readonly RefRO<LocalToWorld> _localToWorld;

        public void OnUpdate(float deltaTime)
        {
            _weapon.ValueRW.CooldownShotsTimer -= deltaTime;

            if (_weapon.ValueRW.CurrentCharges < _weapon.ValueRW.MaxCharges)
            {
                _weapon.ValueRW.RechargeTimer -= deltaTime;

                if (_weapon.ValueRW.RechargeTimer <= 0)
                {
                    _weapon.ValueRW.CurrentCharges++;
                    _weapon.ValueRW.RechargeTimer = _weapon.ValueRW.RechargeTime;
                }
            }
        }

        public void ShootIfNecessary(EntityCommandBuffer commandBuffer)
        {
            if (_weapon.ValueRO.CooldownShotsTimer <= 0f && _weapon.ValueRO.CurrentCharges > 0)
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
                
            commandBuffer.AddComponent<LaserTag>(entity);
            commandBuffer.SetComponent(entity, localTransform);

            var playerForward = math.mul(_localToWorld.ValueRO.Rotation, new float3(0, 1, 0));
            
            commandBuffer.AddComponent<global::VladislavTsurikov.PhysicsVelocity.Runtime.Components.CustomPhysicsVelocity>(entity);
            commandBuffer.SetComponent(entity, new global::VladislavTsurikov.PhysicsVelocity.Runtime.Components.CustomPhysicsVelocity
            {
                Linear = playerForward * _weapon.ValueRW.Speed
            });
            
            commandBuffer.AddComponent(entity, new DamagingComponent
            {
                DamageValue = _weapon.ValueRO.Damage,
                DestroyDamagingEntity = false
            });

            ReduceLaserCharges();
            
            ResetCooldownShotsTimer();
        }
        
        private void ReduceLaserCharges()
        {
            _weapon.ValueRW.CurrentCharges--;
            _weapon.ValueRW.RechargeTimer = _weapon.ValueRW.RechargeTime;
        }
        
        private void ResetCooldownShotsTimer()
        {
            _weapon.ValueRW.CooldownShotsTimer = _weapon.ValueRW.CooldownShots;
        }
    }
}