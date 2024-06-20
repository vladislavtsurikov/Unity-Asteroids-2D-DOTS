using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;
using VladislavTsurikov.EntityCameraProperties.Runtime;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.EntityCameraProperties.Runtime.Enums;
using VladislavTsurikov.EntityOBB.Runtime.Components;
using VladislavTsurikov.EntityScreenTeleport.Runtime.Components;
using VladislavTsurikov.Math.Runtime;
using VladislavTsurikov.Utility.Runtime;
using AABB = Unity.Mathematics.AABB;
using OBBSystem = VladislavTsurikov.EntityOBB.Runtime.Systems.OBBSystem;

namespace VladislavTsurikov.EntityScreenTeleport.Runtime.Systems
{
    [UpdateBefore(typeof(OBBSystem))]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct EntityScreenTeleportSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntityScreenTeleportTag>();
            state.RequireForUpdate<CameraPropertiesComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (IsGameOver(ref state))
            {
                return;
            }
            
            var cameraProperties = SystemAPI.GetSingletonRW<CameraPropertiesComponent>();
            
            new EntityScreenTeleportJob
            {
                CameraProperties = cameraProperties.ValueRO
            }.ScheduleParallel();
        }

        private bool IsGameOver(ref SystemState state)
        {
            foreach (var (_, _)
                     in SystemAPI.Query<RefRO<GameOverEnableableComponent>>().WithEntityAccess())
            {
                return true;
            }

            return false;
        }
        
        [BurstCompile]
        private partial struct EntityScreenTeleportJob : IJobEntity
        {
            public CameraPropertiesComponent CameraProperties;
            
            private void Execute([EntityIndexInQuery] int sortKey, Entity entity, ref LocalTransform localTransform, in OBBComponent obbComponent, in EntityScreenTeleportTag entityScreenTeleportTag)
            {
                float halfWidth = CameraProperties.HalfWidth;
                float halfHeight = CameraProperties.HalfHeight;
                
                var entityObb = GetOBBWithoutRotation(localTransform, obbComponent);
                
                if (CameraProperties.IsHideEntirelyOBB(GetExtendedOBB(entityObb)))
                {
                    if(!CameraProperties.IsPointVisible(localTransform.Position, out ScreenSide side))
                    {
                        TeleportObject(side, ref localTransform, entityObb, halfWidth, halfHeight);
                    }
                }
            }
            
            //For ScreenTeleport you need OBB without Rotation, so that there is no bug that the object teleports back and forth in certain situations constantly
            private OBB GetOBBWithoutRotation(LocalTransform localTransform, OBBComponent obbComponent)
            {
                OBB entityObb = new OBB(localTransform.Position, obbComponent.Obb.Size);
                return entityObb;
            }

            //We need to return OBB, which is larger, so that after the teleport, the entity will not teleport again immediately
            private OBB GetExtendedOBB(OBB entityObb)
            {
                return new OBB(entityObb.Center, entityObb.Size * 1.1f, entityObb.Rotation);
            }

            private void TeleportObject(ScreenSide screenSide, ref LocalTransform localTransform, OBB obb, float halfWidth, float halfHeight)
            {
                var extentsX = obb.Extents.x;
                var extentsY = obb.Extents.y;

                switch (screenSide)
                {
                    case ScreenSide.Left:
                    {
                        float3 newPosition = new float3(halfWidth + extentsX, localTransform.Position.y, localTransform.Position.z);
                        float3 closestPoint = ClosestPoint(CameraProperties.CameraAABB, newPosition);
                        localTransform.Position = new float3(newPosition.x, closestPoint.y, localTransform.Position.z);
                        break;
                    }
                    case ScreenSide.Right:
                    {
                        float3 newPosition = new float3(-halfWidth - extentsX, localTransform.Position.y, localTransform.Position.z);
                        float3 closestPoint = ClosestPoint(CameraProperties.CameraAABB, newPosition);
                        localTransform.Position = new float3(newPosition.x, closestPoint.y, localTransform.Position.z);
                        break;
                    }
                    case ScreenSide.Top:
                    {
                        float3 newPosition = new float3(localTransform.Position.x, -halfHeight - extentsY, localTransform.Position.z);
                        float3 closestPoint = ClosestPoint(CameraProperties.CameraAABB, newPosition);
                        localTransform.Position = new float3(closestPoint.x, newPosition.y, localTransform.Position.z);
                        break;
                    }
                    case ScreenSide.Bottom:
                    {
                        float3 newPosition = new float3(localTransform.Position.x, halfHeight + extentsY, localTransform.Position.z);
                        float3 closestPoint = ClosestPoint(CameraProperties.CameraAABB, newPosition);
                        localTransform.Position = new float3(closestPoint.x, newPosition.y, localTransform.Position.z);
                        break;
                    }
                }
            }

            public float3 ClosestPoint(AABB aabb, float3 position)
            {
                return math.min(aabb.Max, math.max(aabb.Min, position));
            }
        }
    }
}