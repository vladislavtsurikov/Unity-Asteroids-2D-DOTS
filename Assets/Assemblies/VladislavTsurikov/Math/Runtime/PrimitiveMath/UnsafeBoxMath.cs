using Unity.Collections;
using UnityEngine;

namespace VladislavTsurikov.Math.Runtime.PrimitiveMath
{
    public static class UnsafeBoxMath
    {
        public static NativeArray<Vector3> GetCornerPoints(OBB obb)
        {
            return CalcBoxCornerPoints(obb.Center, obb.Size, obb.Rotation);
        }

        public static NativeArray<Vector3> CalcBoxCornerPoints(Vector3 boxCenter, Vector3 boxSize, Quaternion boxRotation)
        {
            Vector3 extents = boxSize * 0.5f;
            Vector3 rightAxis = boxRotation * Vector3.right;
            Vector3 upAxis = boxRotation * Vector3.up;
            Vector3 lookAxis = boxRotation * Vector3.forward;

            NativeArray<Vector3> cornerPoints = new NativeArray<Vector3>(8, Allocator.Temp);
            
            Vector3 faceCenter = boxCenter - lookAxis * extents.z;
            cornerPoints[(int)BoxCorner.FrontTopLeft] = faceCenter - rightAxis * extents.x + upAxis * extents.y;
            cornerPoints[(int)BoxCorner.FrontTopRight] = faceCenter + rightAxis * extents.x + upAxis * extents.y;
            cornerPoints[(int)BoxCorner.FrontBottomRight] = faceCenter + rightAxis * extents.x - upAxis * extents.y;
            cornerPoints[(int)BoxCorner.FrontBottomLeft] = faceCenter - rightAxis * extents.x - upAxis * extents.y;

            faceCenter = boxCenter + lookAxis * extents.z;
            cornerPoints[(int)BoxCorner.BackTopLeft] = faceCenter + rightAxis * extents.x + upAxis * extents.y;
            cornerPoints[(int)BoxCorner.BackTopRight] = faceCenter - rightAxis * extents.x + upAxis * extents.y;
            cornerPoints[(int)BoxCorner.BackBottomRight] = faceCenter - rightAxis * extents.x - upAxis * extents.y;
            cornerPoints[(int)BoxCorner.BackBottomLeft] = faceCenter + rightAxis * extents.x - upAxis * extents.y;
            
            return cornerPoints;
        }
    }
}