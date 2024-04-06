using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Systems
{
    [UpdateInGroup(typeof(RestartSystemGroup))]
    public partial struct DisableGameOverSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameOverEnableableComponent>();
            state.RequireForUpdate<RestartEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (_, entity) 
                     in SystemAPI.Query<RefRO<GameOverEnableableComponent>>().WithEntityAccess().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                state.EntityManager.SetComponentEnabled<GameOverEnableableComponent>(entity, false);
            }
        }
    }
}