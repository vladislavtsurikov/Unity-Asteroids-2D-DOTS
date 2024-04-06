using Unity.Burst;
using Unity.Entities;
using VladislavTsurikov.EntityDestroyer.Runtime;
using VladislavTsurikov.EntityDestroyer.Runtime.Components;
using VladislavTsurikov.Score.Runtime;

namespace VladislavTsurikov.Score.Systems
{
    [UpdateInGroup(typeof(DestroyEntityGroupSystem))]
    public partial struct AccrueScoreOnDestroySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AccrueScoreOnDestroy>();
            state.RequireForUpdate<DestroyEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var scoreComponent in SystemAPI.Query<RefRW<ScoreComponent>>())
            {
                foreach (var (_, accrueScoreOnDestroy) in SystemAPI.Query<RefRW<DestroyEvent>, RefRO<AccrueScoreOnDestroy>>())
                {
                    scoreComponent.ValueRW.Score += accrueScoreOnDestroy.ValueRO.Score;
                }
            }
        }
    }
}