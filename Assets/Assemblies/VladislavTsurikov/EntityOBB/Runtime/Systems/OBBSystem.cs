using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using VladislavTsurikov.EntityOBB.Runtime.Components;
using VladislavTsurikov.Utility.Runtime;

namespace VladislavTsurikov.EntityOBB.Runtime.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class OBBSystem : SystemBase
    {
        private EntityQuery _obbComponentQuery;

        protected override void OnCreate()
        {
            RequireForUpdate<OBBComponent>();

            _obbComponentQuery = GetEntityQuery(ComponentType.ReadWrite<OBBComponent>());
        }
            
        protected override void OnUpdate()
        {
            var updateFromVelocityJob = new OBBJob
            {
                OBBComponentHandle = GetComponentTypeHandle<OBBComponent>(),
                LocalTransformHandle = GetComponentTypeHandle<LocalTransform>(true),
                LinkedEntityGroupHandle = GetBufferTypeHandle<LinkedEntityGroup>(),
                WorldRenderBoundsLookupRO = GetComponentLookup<WorldRenderBounds>(true),
            };

            Dependency = updateFromVelocityJob.ScheduleParallel(_obbComponentQuery, Dependency);
        }

        [BurstCompile]
        public struct OBBJob : IJobChunk
        {
            public ComponentTypeHandle<OBBComponent> OBBComponentHandle;
            [ReadOnly]
            public ComponentTypeHandle<LocalTransform> LocalTransformHandle;
            public BufferTypeHandle<LinkedEntityGroup> LinkedEntityGroupHandle;
                
            [ReadOnly] 
            public ComponentLookup<WorldRenderBounds> WorldRenderBoundsLookupRO;
                
            [BurstCompile]
            public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                NativeArray<OBBComponent> obbComponents = chunk.GetNativeArray(ref OBBComponentHandle);
                NativeArray<LocalTransform> localTransforms = chunk.GetNativeArray(ref LocalTransformHandle);
                    
                BufferAccessor<LinkedEntityGroup> linkedEntityBufferAccessor = chunk.GetBufferAccessor(ref LinkedEntityGroupHandle);

                var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
                while (enumerator.NextEntityIndex(out var i))
                {
                    DynamicBuffer<LinkedEntityGroup> linkedEntityGroups = linkedEntityBufferAccessor[i];
                        
                    Math.Runtime.AABB finalAABB = new Math.Runtime.AABB();
                        
                    for (int j = 0; j < linkedEntityGroups.Length; j++)
                    {
                        if (WorldRenderBoundsLookupRO.HasComponent(linkedEntityGroups[j].Value))
                        {
                            WorldRenderBounds worldRenderBounds = WorldRenderBoundsLookupRO[linkedEntityGroups[j].Value];
                                
                            Math.Runtime.AABB modelAABB = new Math.Runtime.AABB(worldRenderBounds.Value.ToBounds());

                            if (finalAABB.IsValid)
                            {
                                finalAABB.Encapsulate(modelAABB);
                            }
                            else
                            {
                                finalAABB = modelAABB;
                            }
                        }
                    }

                    obbComponents[i] = new OBBComponent()
                    {
                        Obb = new Math.Runtime.OBB(finalAABB, localTransforms[i].Rotation)
                    };
                }
            }
        }

#if UNITY_EDITOR
        [UnityEditor.DrawGizmo(UnityEditor.GizmoType.NonSelected)]
        public static void DrawGizmos(DebugOBB debugObb, UnityEditor.GizmoType type)
        {
            if (debugObb.isActiveAndEnabled)
            {
                if (World.DefaultGameObjectInjectionWorld == null)
                {
                    return;
                }

                var renderer = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<OBBSystem>();
                if (renderer != null)
                {
                    renderer.DrawGizmos();
                }
            }
        }
        
        private void DrawGizmos()
        {
            var obbComponents = _obbComponentQuery.ToComponentDataArray<OBBComponent>(Allocator.TempJob);
                
            for (int i = 0; i != obbComponents.Length; i++)
            {
                OBBComponent obbComponent = obbComponents[i];
                    
                DebugDrawObb(obbComponent.Obb.Center, obbComponent.Obb.Rotation, obbComponent.Obb.Size);
            }
                
            obbComponents.Dispose();
        }
            
        private void DebugDrawObb(Vector3 center, Quaternion orientation, Vector3 size)
        {
            GizmosEx.PushMatrix(Matrix4x4.TRS(center, orientation, size));
            GizmosEx.PushColor(Color.blue);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            GizmosEx.PopColor();
            GizmosEx.PopMatrix();
        }
#endif
    } 
}