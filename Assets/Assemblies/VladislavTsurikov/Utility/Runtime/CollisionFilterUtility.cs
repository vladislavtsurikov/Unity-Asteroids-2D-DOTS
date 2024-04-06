using Unity.Physics;
using UnityEngine;

namespace VladislavTsurikov.Utility.Runtime
{
    public static class CollisionFilterUtility
    {
        public static CollisionFilter Create(LayerMask belongsToMask, LayerMask collidesWithMask)
        {
            var filter = new CollisionFilter
            {
                BelongsTo = (uint)belongsToMask.value,
                CollidesWith = (uint)collidesWithMask.value
            };

            return filter;
        }
        
        public static CollisionFilter Create(int belongsToMask, int collidesWithMask)
        {
            var filter = new CollisionFilter
            {
                BelongsTo = (uint)belongsToMask,
                CollidesWith = (uint)collidesWithMask
            };

            return filter;
        }
    }
}