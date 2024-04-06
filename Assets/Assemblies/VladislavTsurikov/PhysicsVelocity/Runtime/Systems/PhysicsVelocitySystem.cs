using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using VladislavTsurikov.PhysicsVelocity.Runtime.Components;

namespace VladislavTsurikov.PhysicsVelocity.Runtime.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct PhysicsVelocitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CustomPhysicsVelocity>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
             foreach (var queryEnumerable in SystemAPI.Query<RefRW<CustomPhysicsVelocity>, RefRW<LocalTransform>>())
             {
                 queryEnumerable.Item2.ValueRW.Position += queryEnumerable.Item1.ValueRO.Linear * deltaTime;
             }
        }
    }
}