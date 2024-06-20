using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.Weapons.Runtime.Bullet.Components;
using BulletWeaponAspect = VladislavTsurikov.Weapons.Runtime.Bullet.Aspects.BulletWeaponAspect;

namespace VladislavTsurikov.Weapons.Runtime.Bullet.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class BulletShootingSystem : SystemBase
    {
        private WeaponsAction _inputAction;
        
        protected override void OnCreate()
        {
            RequireForUpdate<BulletWeaponComponent>();
            _inputAction = new WeaponsAction();
        }
        
        protected override void OnStartRunning()
        {
            _inputAction.Enable();
            _inputAction.Weapons.FireBullet.performed += _ => OnFire();
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
        
            private void Execute(BulletWeaponAspect weaponAspect)
            {
                weaponAspect.OnUpdate(DeltaTime);
            }
        }

        [BurstCompile]
        private partial struct ShootingJob : IJobEntity
        {
            public EntityCommandBuffer CommandBuffer;

            private void Execute(BulletWeaponAspect weaponAspect)
            {
                weaponAspect.ShootIfNecessary(CommandBuffer);
            }
        }
    }
}