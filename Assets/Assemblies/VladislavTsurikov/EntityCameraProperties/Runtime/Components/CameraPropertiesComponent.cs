using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using VladislavTsurikov.EntityCameraProperties.Runtime.Enums;
using VladislavTsurikov.EntityCameraProperties.Runtime.Utility;

namespace VladislavTsurikov.EntityCameraProperties.Runtime.Components
{
    public struct CameraPropertiesComponent : IComponentData
    {
        public float Width;
        public float Height;
        
        public int PixelWidth;
        public int PixelHeight;

        public float3 Center;
        
        public Plane PlaneLeft;
        public Plane PlaneRight;
        public Plane PlaneTop;
        public Plane PlaneBottom;

        public Matrix4x4 CameraToWorldMatrix;
        public Matrix4x4 ProjectionMatrix;

        public float HalfWidth => Width / 2;
        public float HalfHeight => Height / 2;

        public AABB CameraAABB =>
            new()
            {
                Center = Center,
                Extents = new float3(HalfWidth, HalfHeight, 1000f)
            };

        public bool IsPointVisible(float3 point)
        {
            return CameraUtils.IsPointVisible(point, CameraToWorldMatrix, ProjectionMatrix);
        }

        public bool IsPointVisible(float3 worldPoint, out ScreenSide invisibleScreenSide)
        {
            return CameraUtils.IsPointVisible(worldPoint, CameraToWorldMatrix, ProjectionMatrix, out invisibleScreenSide);
        }
    }
}