using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.OutOfViewEntityDestroyer.Components;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.OutOfViewEntityDestroyer.Authorings
{
    public class OutOfViewDestroyAuthoring : MonoBehaviour
    {
        public class Baker : Baker<OutOfViewDestroyAuthoring>
        {
            public override void Bake(OutOfViewDestroyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new OutOfViewDestroyTag());
            }
        }
    }
}