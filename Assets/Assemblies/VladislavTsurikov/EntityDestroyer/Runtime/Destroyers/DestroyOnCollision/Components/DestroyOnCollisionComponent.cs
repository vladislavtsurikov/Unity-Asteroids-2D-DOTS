using Unity.Entities;
using Unity.Physics;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.DestroyOnCollision.Components
{
    public struct DestroyOnCollisionComponent : IComponentData
    {
        public CollisionFilter CollisionFilter;
    }
}