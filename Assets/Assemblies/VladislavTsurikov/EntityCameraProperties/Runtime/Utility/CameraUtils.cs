using Unity.Mathematics;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.EntityCameraProperties.Runtime.Enums;

namespace VladislavTsurikov.EntityCameraProperties.Runtime.Utility
{
    public static class CameraUtils
    {
        public static float3 WorldToViewportPoint(float3 worldPoint, float4x4 cameraToWorldMatrix, float4x4 projectionMatrix)
        {
            float4 clipSpacePos = math.mul(projectionMatrix, math.mul(cameraToWorldMatrix, new float4(worldPoint, 1.0f)));
            float3 normalizedDevicePos = clipSpacePos.xyz / clipSpacePos.w;
            return 0.5f * normalizedDevicePos + new float3(0.5f, 0.5f, 0.5f);
        }
        
        public static float3 WorldToViewportPoint(float3 worldPoint, CameraPropertiesComponent cameraProperties)
        {
            float4 clipSpacePos = math.mul(cameraProperties.ProjectionMatrix, math.mul(cameraProperties.CameraToWorldMatrix, new float4(worldPoint, 1.0f)));
            float3 normalizedDevicePos = clipSpacePos.xyz / clipSpacePos.w;
            return 0.5f * normalizedDevicePos + new float3(0.5f, 0.5f, 0.5f);
        }
        
        public static bool IsPointVisible(float3 point, float4x4 cameraToWorldMatrix, float4x4 projectionMatrix, out ScreenSide invisibleScreenSide)
        {
            float3 screenPoint = WorldToViewportPoint(point, cameraToWorldMatrix, projectionMatrix);

            invisibleScreenSide = ScreenSide.None;

            FindInvisibleSide(screenPoint.x, ScreenSide.Left, ScreenSide.Right, ref invisibleScreenSide);
            FindInvisibleSide(screenPoint.y, ScreenSide.Bottom, ScreenSide.Top, ref invisibleScreenSide);
            FindInvisibleSide(screenPoint.z, ScreenSide.Behind, ScreenSide.None, ref invisibleScreenSide);

            return invisibleScreenSide == ScreenSide.None;
            
            void FindInvisibleSide(float axisValue, ScreenSide negativeSide, ScreenSide positiveSide, ref ScreenSide invisibleSide)
            {
                if (invisibleSide != ScreenSide.None)
                {
                    return;
                }
                
                if (axisValue < 0)
                {
                    invisibleSide = negativeSide;
                }
                else if (axisValue > 1)
                {
                    invisibleSide = positiveSide;
                }
            }
        }
        
        public static bool IsPointVisible(float3 point, float4x4 cameraToWorldMatrix, float4x4 projectionMatrix)
        {
            float3 screenPoint = WorldToViewportPoint(point, cameraToWorldMatrix, projectionMatrix);
            
            return screenPoint is { z: > 0, x: > 0 } && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }
    }
}