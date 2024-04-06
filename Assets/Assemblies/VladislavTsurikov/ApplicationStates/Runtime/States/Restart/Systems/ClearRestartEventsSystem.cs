using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Systems
{
    [UpdateInGroup(typeof(RestartSystemGroup), OrderLast = true)]
    public partial struct ClearRestartEventsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RestartEvent>();
        }
    
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entities = SystemAPI.QueryBuilder()
                .WithAll<RestartEvent>()
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