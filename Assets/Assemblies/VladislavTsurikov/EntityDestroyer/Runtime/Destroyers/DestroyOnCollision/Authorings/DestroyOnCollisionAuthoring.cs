using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.DestroyOnCollision.Components;
using VladislavTsurikov.EntityLayer.Runtime.Utility;
using VladislavTsurikov.Utility.Runtime;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.DestroyOnCollision.Authorings
{
    public class DestroyOnCollisionAuthoring : MonoBehaviour
    {
        public LayerMask DestroyOnCollisionLayer;
        
        public class Baker : Baker<DestroyOnCollisionAuthoring>
        {
            public override void Bake(DestroyOnCollisionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new DestroyOnCollisionComponent
                {
                    CollisionFilter = CollisionFilterUtility.Create(
                        LayerMaskUtility.ConvertIndexLayerToLayerMask(authoring.gameObject.layer), (int)authoring.DestroyOnCollisionLayer)
                });
            }
        }
    }
}