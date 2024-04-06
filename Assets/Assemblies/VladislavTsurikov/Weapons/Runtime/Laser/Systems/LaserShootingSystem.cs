using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.Weapons.Laser.Components;
using LaserWeaponAspect = VladislavTsurikov.Weapons.Laser.Aspects.LaserWeaponAspect;

namespace VladislavTsurikov.Weapons.Laser.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class LaserShootingSystem : SystemBase
    {
        private WeaponsAction _inputAction;
        
        protected override void OnCreate()
        {
            RequireForUpdate<LaserWeaponComponent>();
            _inputAction = new WeaponsAction();
        }
        
        protected override void OnStartRunning()
        {
            _inputAction.Enable();
            _inputAction.Weapons.FireLaser.performed += _ => OnFire();
        }

        protected override void OnStopRunning()
        {
            _inputAction.Disable();
        }

        private void OnFire()
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer entityCommandBuffer = ecbSingleton.CreateCommandBuffer(World.Unmanaged);
            
            new ShootingJob
            {
                CommandBuffer = entityCommandBuffer,
            
            }.Run();
        }

        protected override void OnUpdate()
        {
            CompleteDependency();
                
            new UpdateWeaponJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
            
            }.Run();
        }
        
        [BurstCompile]
        private partial struct UpdateWeaponJob : IJobEntity
        {
            public float DeltaTime;
        
            private void Execute(LaserWeaponAspect weaponAspect)
            {
                weaponAspect.OnUpdate(DeltaTime);
            }
        }
        
        [BurstCompile]
        private partial struct ShootingJob : IJobEntity
        {
            public EntityCommandBuffer CommandBuffer;
        
            private void Execute(LaserWeaponAspect weaponAspect)
            {
                weaponAspect.ShootIfNecessary(CommandBuffer);
            }
        }
    }
}