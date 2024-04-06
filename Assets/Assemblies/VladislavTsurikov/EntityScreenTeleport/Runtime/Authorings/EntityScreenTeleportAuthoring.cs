using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.EntityScreenTeleport.Runtime.Components;

namespace VladislavTsurikov.EntityScreenTeleport.Runtime.Authorings
{
    public class EntityScreenTeleportAuthoring : MonoBehaviour
    {
        public class Baker : Baker<EntityScreenTeleportAuthoring>
        {
            public override void Bake(EntityScreenTeleportAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new EntityScreenTeleportTag());
            }
        }
    }
}