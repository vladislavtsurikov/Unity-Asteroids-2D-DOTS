using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;
using VladislavTsurikov.Core.Runtime.Components;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.EntityOBB.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Aspects
{
    public enum Side
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }
    
    public readonly partial struct IntervalSpawnerAspect : IAspect
    {
        private readonly RefRW<IntervalSpawnerComponent> _spawnerComponent;
        private readonly RefRW<SpawnerRandom> _spawnerRandom;
        private readonly RefRW<SpawnerHashCode> _hashCodeComponent;
        private readonly DynamicBuffer<PrototypeComponent> _prototypes;
        
        public void SpawnIfNecessary(EntityCommandBuffer commandBuffer, CameraPropertiesComponent cameraPropertiesComponent, float deltaTime)
        {
            _spawnerComponent.ValueRW.UpdateTimer(deltaTime);
            
            if (_spawnerComponent.ValueRW.CanSpawn())
            {
                Spawn(commandBuffer, cameraPropertiesComponent);
                    
                _spawnerComponent.ValueRW.ResetTimer();
            }
        }

        private void Spawn(EntityCommandBuffer commandBuffer, CameraPropertiesComponent cameraPropertiesComponent)
        { 
            PrototypeComponent prototypeComponent = PrototypeComponent.GetRandomPrototype(_prototypes, ref _spawnerRandom.ValueRW.Value);

            Entity newEntity = commandBuffer.Instantiate(prototypeComponent.Prefab);
                
            float3 randomPosition = GetRandomPositionOnScreenBorder((Side)_spawnerRandom.ValueRW.Value.NextInt(0, 4), prototypeComponent, cameraPropertiesComponent);
                
            LocalTransform localTransform = new LocalTransform
            {
                Position = randomPosition,
                Rotation = quaternion.RotateZ(_spawnerRandom.ValueRW.Value.NextFloat(0f, 360)),
                Scale = 1f
            };
                
            commandBuffer.SetComponent(newEntity, localTransform);
            
            commandBuffer.AddComponent(newEntity, new Spawnable(_hashCodeComponent.ValueRO.Value));

            _spawnerComponent.ValueRW.IsSpawned = true;
        }

        private float3 GetRandomPositionOnScreenBorder(Side side, PrototypeComponent prototypeComponent, CameraPropertiesComponent cameraPropertiesComponent)
        {
            float2 offset = new float2(prototypeComponent.PrefabExtents.x, prototypeComponent.PrefabExtents.y);  // Offset to spawn just outside the screen
            float randomX = 0f;
            float randomY = 0f;
            
            switch (side)
            {
                case Side.Left:
                    randomY = _spawnerRandom.ValueRW.Value.NextFloat(-cameraPropertiesComponent.Height / 2, cameraPropertiesComponent.Height / 2 );
                    randomX = -cameraPropertiesComponent.Width / 2 - offset.x;
                    break;
                case Side.Top:
                    randomX = _spawnerRandom.ValueRW.Value.NextFloat(-cameraPropertiesComponent.Width / 2, cameraPropertiesComponent.Width / 2);
                    randomY = cameraPropertiesComponent.Height / 2 + offset.y;
                    break;
                case Side.Right:
                    randomY = _spawnerRandom.ValueRW.Value.NextFloat(-cameraPropertiesComponent.Height / 2, cameraPropertiesComponent.Height / 2);
                    randomX = cameraPropertiesComponent.Width / 2 + offset.x;
                    break;
                case Side.Bottom:
                    randomX = _spawnerRandom.ValueRW.Value.NextFloat(-cameraPropertiesComponent.Width / 2, cameraPropertiesComponent.Width / 2);
                    randomY = -cameraPropertiesComponent.Height / 2 - offset.y;
                    break;
            }
            
            float3 randomPosition = new float3(randomX, randomY, 0);
            return randomPosition;
        }
    }
}