using Unity.Entities;
using UnityEngine;

namespace VladislavTsurikov.EntityCameraProperties.Runtime.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct UpdateCameraProperties : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Components.CameraPropertiesComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            Camera camera = Camera.main;
            
            if (camera == null)
            {
                return;
            }
            
            RefRW<Components.CameraPropertiesComponent> cameraProperties = SystemAPI.GetSingletonRW<Components.CameraPropertiesComponent>();

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

            cameraProperties.ValueRW.Center = camera.transform.position;
            
            if (camera.orthographic)
            {
                float orthoSize = camera.orthographicSize;
                cameraProperties.ValueRW.Width = orthoSize * 2f * camera.aspect;
                cameraProperties.ValueRW.Height = orthoSize * 2f;
            }
            else
            {
                float distanceFrontPlane = planes[5].distance;

                float heightSize = 2f * distanceFrontPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                float widthSize = heightSize * camera.aspect;
                cameraProperties.ValueRW.Width = widthSize;
                cameraProperties.ValueRW.Height = heightSize;
            }

            cameraProperties.ValueRW.PixelWidth = camera.pixelWidth;
            cameraProperties.ValueRW.PixelHeight = camera.pixelHeight;

            cameraProperties.ValueRW.PlaneLeft = planes[0];
            cameraProperties.ValueRW.PlaneRight = planes[1];
            cameraProperties.ValueRW.PlaneTop = planes[2];
            cameraProperties.ValueRW.PlaneBottom = planes[3];
            
            cameraProperties.ValueRW.CameraToWorldMatrix = camera.cameraToWorldMatrix;
            cameraProperties.ValueRW.ProjectionMatrix = camera.projectionMatrix;
        }
    }
}