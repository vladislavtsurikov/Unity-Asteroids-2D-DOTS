using Unity.Entities;
using Unity.Mathematics;

namespace VladislavTsurikov.PhysicsVelocity.Runtime.Components
{
    public struct CustomPhysicsVelocity : IComponentData
    {
        public float3 Linear;
    }
}