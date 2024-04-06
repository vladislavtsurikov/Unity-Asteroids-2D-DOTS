using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace VladislavTsurikov.EntityOBB.Runtime.Bakers
{
    public class SpriteRendererBaker : Baker<SpriteRenderer>
    {
        public override void Bake(SpriteRenderer authoring)
        {
            var mainEntity = GetEntity(TransformUsageFlags.Renderable);

            var bounds = authoring.bounds;
            
            AddComponent(mainEntity, new RenderBounds
            {
                Value = new AABB
                {
                    Center = bounds.center,
                    Extents = bounds.extents,
                }
            });
        }
    }
}