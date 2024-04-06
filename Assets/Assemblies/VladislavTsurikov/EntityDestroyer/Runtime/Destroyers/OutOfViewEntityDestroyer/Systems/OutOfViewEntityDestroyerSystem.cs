using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.OutOfViewEntityDestroyer.Components;
using VladislavTsurikov.EntityOBB.Runtime.Components;
using VladislavTsurikov.Utility.Runtime;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.OutOfViewEntityDestroyer.Systems
{
    public partial struct OutOfViewEntityDestroyerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<OutOfViewDestroyTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            var cameraProperties = SystemAPI.GetSingletonRW<CameraPropertiesComponent>();
            
            foreach (var (_, obbComponent, entity) in 
                     SystemAPI.Query<RefRO<OutOfViewDestroyTag>, RefRW<OBBComponent>>().WithEntityAccess())
            {
                if (cameraProperties.ValueRO.IsHideEntirelyOBB(obbComponent.ValueRO.Obb))
                {
                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
        }
    }
}