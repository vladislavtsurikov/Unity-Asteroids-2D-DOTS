using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components;

namespace VladislavTsurikov.Asteroids.Runtime.Authorings
{
    public class HealthAuthoring : MonoBehaviour
    {
        public int Health = 1;
        
        public class Baker : Baker<HealthAuthoring>
        {
            public override void Bake(HealthAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new HealthComponent { Value = authoring.Health });
            }
        }
    }
}