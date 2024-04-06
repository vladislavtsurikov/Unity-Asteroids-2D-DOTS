using Unity.Collections;
using UnityEngine;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.Math.Runtime;
using VladislavTsurikov.Math.Runtime.PrimitiveMath;

namespace VladislavTsurikov.Utility.Runtime
{
    public static class CameraPropertiesExtensions
    {
        public static bool IsVisibleEntirelyOBB(this CameraPropertiesComponent cameraPropertiesComponent, OBB obb)
        {
            NativeArray<Vector3> obbCornerPoints = UnsafeBoxMath.GetCornerPoints(obb);

            foreach (var cornerPoint in obbCornerPoints)
            {
                if (!cameraPropertiesComponent.IsPointVisible(cornerPoint))
                {
                    obbCornerPoints.Dispose();
                    return false;
                }
            }
            
            obbCornerPoints.Dispose();
            return true;
        }

        public static bool IsHideEntirelyOBB(this CameraPropertiesComponent cameraPropertiesComponent, OBB obb)
        {
            NativeArray<Vector3> obbCornerPoints = UnsafeBoxMath.GetCornerPoints(obb);

            foreach (var cornerPoint in obbCornerPoints)
            {
                if (cameraPropertiesComponent.IsPointVisible(cornerPoint))
                {
                    obbCornerPoints.Dispose();
                    return false;
                }
            }
            
            obbCornerPoints.Dispose();
            return true;
        }
    }
}