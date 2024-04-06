using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Aspects;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Systems
{
    [UpdateInGroup(typeof(SpawnerSystemGroup))]
    public partial struct IntervalSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<IntervalSpawnerComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (IsGameOver(ref state))
            {
                return;
            }
            
            RefRW<CameraPropertiesComponent> cameraProperties = SystemAPI.GetSingletonRW<CameraPropertiesComponent>();

            SpawnJob spawnJob = new SpawnJob
            {
                CameraPropertiesComponent = cameraProperties.ValueRO,
                CommandBuffer = new EntityCommandBuffer(Allocator.TempJob),
                DeltaTime = SystemAPI.Time.DeltaTime,
            };
                
            spawnJob.Run();
                
            spawnJob.CommandBuffer.Playback(state.EntityManager);
            spawnJob.CommandBuffer.Dispose();
        }

        private bool IsGameOver(ref SystemState state)
        {
            foreach (var (_, _)
                     in SystemAPI.Query<RefRO<GameOverEnableableComponent>>().WithEntityAccess())
            {
                return true;
            }

            return false;
        }
    }

    [BurstCompile]
    partial struct SpawnJob : IJobEntity
    {
        public CameraPropertiesComponent CameraPropertiesComponent;
        public EntityCommandBuffer CommandBuffer;
        public float DeltaTime;
        public bool ForceSpawn;

        [BurstCompile]
        private void Execute(IntervalSpawnerAspect aspect)
        {
            aspect.SpawnIfNecessary(CommandBuffer, CameraPropertiesComponent, DeltaTime);
        }
    }
}