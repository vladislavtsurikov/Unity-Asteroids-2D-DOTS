using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.Asteroids.Runtime.Components;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime;
using VladislavTsurikov.EntitySpawning.Runtime.Components;
using VladislavTsurikov.PhysicsVelocity.Runtime.Components;
using VladislavTsurikov.Utility.Runtime;

namespace VladislavTsurikov.Asteroids.Runtime.Systems
{
    [UpdateInGroup(typeof(FindSpawnableComponentSystemGroup))]
    public partial struct RandomVelocityDirectionOnSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Spawnable>();
            state.RequireForUpdate<RandomVelocityDirectionOnSpawnComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            RefRW<CameraPropertiesComponent> cameraProperties = SystemAPI.GetSingletonRW<CameraPropertiesComponent>();

            foreach (var (randomVelocityDirectionOnSpawn, spawnerRandom, spawnerHashCodeComponent, _) 
                     in SystemAPI.Query<RefRO<RandomVelocityDirectionOnSpawnComponent>, RefRW<SpawnerRandom>, RefRO<SpawnerHashCode>>().WithEntityAccess())
            {
                foreach (var (spawnable, localTransform, entity) in SystemAPI
                             .Query<RefRO<Spawnable>, RefRO<LocalTransform>>().WithEntityAccess())
                {
                    if (spawnerHashCodeComponent.ValueRO.Value == spawnable.ValueRO.SpawnerHashCode)
                    {
                        SetRandomVelocityDirection(commandBuffer, entity, localTransform.ValueRO.Position, spawnerRandom, randomVelocityDirectionOnSpawn.ValueRO.Speed, cameraProperties.ValueRO);
                    }
                }
            }
            
            commandBuffer.Playback(state.EntityManager);
            commandBuffer.Dispose();
        }
        
        private void SetRandomVelocityDirection(EntityCommandBuffer commandBuffer, Entity entity, float3 position, RefRW<SpawnerRandom> spawnerRandom, float speed, CameraPropertiesComponent cameraPropertiesComponent)
        {
            float3 direction = GetRandomDirection(position, cameraPropertiesComponent, spawnerRandom);

            direction *= speed;

            commandBuffer.AddComponent(entity, new CustomPhysicsVelocity{Linear = direction});
        }
        
        private float3 GetRandomDirection(float3 position, CameraPropertiesComponent cameraPropertiesComponent, RefRW<SpawnerRandom> spawnerRandom)
        {
            //It is necessary to reduce the screen size so that the Random Position is closer to the center of the screen
            float newWidth = cameraPropertiesComponent.Width / 1.5f;
            float newHeight = cameraPropertiesComponent.Height / 1.5f;
            
            float halfWidth = newWidth / 2;
            float halfHeight = newHeight / 2;

            float3 randomPosition = new float3(spawnerRandom.ValueRW.Value.NextFloat(cameraPropertiesComponent.Center.x - halfWidth, cameraPropertiesComponent.Center.x + halfWidth), 
                spawnerRandom.ValueRW.Value.NextFloat(cameraPropertiesComponent.Center.y - halfHeight, cameraPropertiesComponent.Center.y + halfHeight), 0f);

            float3 direction = (randomPosition - position).Normalize();

            return direction;
        }
    }
}