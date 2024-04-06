using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Systems
{
    [UpdateInGroup(typeof(RestartSystemGroup))]
    public partial struct DestroyOnRestartSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RestartEvent>();
            state.RequireForUpdate<DestroyOnRestart>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entities = SystemAPI.QueryBuilder()
                .WithAll<DestroyOnRestart>()
                .Build()
                .ToEntityArray(Allocator.TempJob);
            
            for (int i = 0; i < entities.Length; i++)
            {
                state.EntityManager.DestroyEntity(entities[i]);
            }

            entities.Dispose();
        }
    }
}