using Unity.Entities;
using UnityEngine;

namespace VladislavTsurikov.EntityLayer.Runtime.Bakers
{
    //Adds an EntityLayer to all Entities that have a Transform. By default, Entity has no layers
    public class EntityLayerBaker : Baker<Transform>
    {
        public override void Bake(Transform authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new Components.EntityLayer(authoring.gameObject.layer));
        }
    }
}