using Unity.Physics;

namespace VladislavTsurikov.Utility.Runtime
{
    public static class CollisionFilterExtensions
    {
        public static bool IsCollisionEnabled(this CollisionFilter collisionFilter, int layerMask)
        {
            return (collisionFilter.CollidesWith & (uint)layerMask) != 0;
        }
    }
}