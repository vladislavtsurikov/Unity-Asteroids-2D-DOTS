using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using VladislavTsurikov.Math.Runtime.Extensions;
using VladislavTsurikov.Math.Runtime.PrimitiveMath;

namespace VladislavTsurikov.Math.Runtime
{
    [Serializable]
    public struct OBB
    {
        [SerializeField]
        private Vector3 _size;
        [SerializeField]
        private Vector3 _center;
        [SerializeField]
        private Quaternion _rotation;
        [SerializeField]
        private bool _isValid;

        public bool IsValid => _isValid;
        public Vector3 Center { get => _center;
            set => _center = value;
        }
        public Vector3 Size { get => _size;
            set => _size = value.Abs();
        }
        public Vector3 Extents => Size * 0.5f;
        public Quaternion Rotation { get => _rotation;
            set => _rotation = value;
        }
        public Matrix4x4 RotationMatrix => Matrix4x4.TRS(Vector3.zero, _rotation, Vector3.one);
        public Vector3 Right => _rotation * Vector3.right;
        public Vector3 Up => _rotation * Vector3.up;
        public Vector3 Look => _rotation * Vector3.forward;

        public OBB(Vector3 center, Vector3 size)
        {
            _center = center;
            _size = size.Abs();
            _rotation = Quaternion.identity;
            _isValid = true;
        }

        public OBB(Vector3 center, Vector3 size, Quaternion rotation)
        {
            _center = center;
            _size = size.Abs();
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(Vector3 center, Quaternion rotation)
        {
            _center = center;
            _size = Vector3.zero;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(Quaternion rotation)
        {
            _center = Vector3.zero;
            _size = Vector3.zero;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(Bounds bounds, Quaternion rotation)
        {
            _center = bounds.center;
            _size = bounds.size.Abs();
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(AABB aabb)
        {
            _center = aabb.Center;
            _size = aabb.Size;
            _rotation = Quaternion.identity;
            _isValid = true;
        }

        public OBB(AABB aabb, Quaternion rotation)
        {
            _center = aabb.Center;
            _size = aabb.Size;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(AABB modelSpaceAABB, Transform worldTransform)
        {
            _size = Vector3.Scale(modelSpaceAABB.Size, worldTransform.lossyScale).Abs();
            _center = worldTransform.TransformPoint(modelSpaceAABB.Center);
            _rotation = worldTransform.rotation;
            _isValid = true;
        }

        public OBB(OBB copy)
        {
            _size = copy._size;
            _center = copy._center;
            _rotation = copy._rotation;
            _isValid = copy._isValid;
        }

        public static OBB GetInvalid()
        {
            return new OBB();
        }

        public void Inflate(float amount)
        {
            Size += Vector3Ex.FromValue(amount);
        }

        public bool IntersectsOBB(OBB otherOBB)
        {
            return BoxMath.BoxIntersectsBox(_center, _size, _rotation, otherOBB.Center, otherOBB.Size, otherOBB.Rotation);
        }
    }
}