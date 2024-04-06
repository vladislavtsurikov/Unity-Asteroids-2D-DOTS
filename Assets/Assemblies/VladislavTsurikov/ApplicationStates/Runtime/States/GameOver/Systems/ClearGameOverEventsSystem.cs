using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Systems
{
    [UpdateInGroup(typeof(GameOverSystemGroup), OrderLast = true)]
    public partial struct ClearGameOverEventsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameOverEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entities = SystemAPI.QueryBuilder()
                .WithAll<GameOverEvent>()
                .Build()
                .ToEntityArray(Allocator.TempJob);

            foreach (var entity in entities)
            {
                state.EntityManager.DestroyEntity(entity);
            }

            entities.Dispose();
        }
    }
}