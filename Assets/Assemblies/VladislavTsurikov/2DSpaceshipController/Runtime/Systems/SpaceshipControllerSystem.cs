using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov._2DSpaceshipController.Runtime.Components;
using VladislavTsurikov.PhysicsVelocity.Runtime.Components;

namespace VladislavTsurikov._2DSpaceshipController.Runtime.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct SpaceshipControllerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpaceshipController>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerController, playerControllerInput, physicsVelocity, localTransform) in SystemAPI
                         .Query<RefRO<SpaceshipController>, RefRO<SpaceshipControllerInput>, RefRW<CustomPhysicsVelocity>, RefRW<LocalTransform>>())
            {
                RotateEntity(localTransform, playerController, playerControllerInput, SystemAPI.Time.DeltaTime);
                MoveEntity(localTransform, playerController, playerControllerInput, physicsVelocity, SystemAPI.Time.DeltaTime);
            }
        }

        private static void MoveEntity(RefRW<LocalTransform> localTransform, RefRO<SpaceshipController> playerController, RefRO<SpaceshipControllerInput> playerControllerInput,
            RefRW<CustomPhysicsVelocity> physicsVelocity, float deltaTime)
        {
            float3 forwardDirection = math.mul(localTransform.ValueRO.Rotation, new float3(0, 1, 0));

            float moveAcceleration = playerController.ValueRO.MoveAcceleration * math.max(0, playerControllerInput.ValueRO.Value.y) * deltaTime;

            physicsVelocity.ValueRW.Linear += forwardDirection * moveAcceleration;
        }

        private static void RotateEntity(RefRW<LocalTransform> localTransform, RefRO<SpaceshipController> playerController, RefRO<SpaceshipControllerInput> playerControllerInput, float deltaTime)
        {
            quaternion currentRotation = localTransform.ValueRW.Rotation;

            float angle = playerController.ValueRO.RotationSpeed * playerControllerInput.ValueRO.Value.x * deltaTime;

            quaternion deltaRotation = quaternion.AxisAngle(-math.forward(), math.radians(angle));

            quaternion newRotation = math.mul(currentRotation, deltaRotation);

            localTransform.ValueRW.Rotation = newRotation;
        }
    }
}