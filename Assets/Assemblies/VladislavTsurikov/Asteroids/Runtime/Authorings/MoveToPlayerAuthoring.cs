using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Asteroids.Runtime.Components;

namespace VladislavTsurikov.Asteroids.Runtime.Authorings
{
    public class MoveToPlayerAuthoring : MonoBehaviour
    {
        public float Speed = 1f;
        
        public class Baker : Baker<MoveToPlayerAuthoring>
        {
            public override void Bake(MoveToPlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MoveToPlayerComponent(authoring.Speed));
            }
        }
    }
}