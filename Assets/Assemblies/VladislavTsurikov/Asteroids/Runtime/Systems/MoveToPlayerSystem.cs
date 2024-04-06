using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using VladislavTsurikov.Asteroids.Runtime.Components;
using VladislavTsurikov.MoveTowardsTarget.Runtime.Components;

namespace VladislavTsurikov.Asteroids.Runtime.Systems
{
    public partial struct MoveToPlayerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<global::VladislavTsurikov._2DSpaceshipController.Runtime.Components.SpaceshipController>();
            state.RequireForUpdate<MoveToPlayerComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            Entity playerControllerEntity = SystemAPI.GetSingletonEntity<global::VladislavTsurikov._2DSpaceshipController.Runtime.Components.SpaceshipController>();

            foreach (var (moveToPlayerComponent, entity) in 
                     SystemAPI.Query<RefRO<MoveToPlayerComponent>>().WithEntityAccess())
            {
                commandBuffer.AddComponent(entity, new TargetComponent(playerControllerEntity, moveToPlayerComponent.ValueRO.Speed));
                commandBuffer.RemoveComponent<MoveToPlayerComponent>(entity);
            }
            
            commandBuffer.Playback(state.EntityManager);
            commandBuffer.Dispose();
        }
    }
}