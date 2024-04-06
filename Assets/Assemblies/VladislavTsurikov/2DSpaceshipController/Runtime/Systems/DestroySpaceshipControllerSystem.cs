using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;
using VladislavTsurikov.EntityDestroyer.Runtime;
using VladislavTsurikov.EntityDestroyer.Runtime.Components;

namespace VladislavTsurikov._2DSpaceshipController.Runtime.Systems
{
    [UpdateInGroup(typeof(DestroyEntityGroupSystem))]
    public partial struct DestroySpaceshipControllerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Components.SpaceshipController>();
            state.RequireForUpdate<DestroyEvent>();
            state.RequireForUpdate<GameOverEnableableComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            EntityCommandBuffer commandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
                        
            foreach (var (_, _, _) in SystemAPI.Query<RefRO<Components.SpaceshipController>, RefRO<DestroyEvent>>().WithEntityAccess())
            {
                Entity gameOverEventEntity = commandBuffer.CreateEntity();
                commandBuffer.AddComponent<GameOverEvent>(gameOverEventEntity);
            }
        }
    }
}