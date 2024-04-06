using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.EntityOBB.Runtime.Components;

namespace VladislavTsurikov.EntityOBB.Runtime.Authorings
{
    public class OBBAuthoring : MonoBehaviour
    {
        public class Baker : Baker<OBBAuthoring>
        {
            public override void Bake(OBBAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new OBBComponent());
            }
        }
    }
}