using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Asteroids.Runtime.Components;

namespace VladislavTsurikov.Asteroids.Runtime.Authorings
{
    public class RandomVelocityDirectionOnSpawnAuthoring : MonoBehaviour
    {
        public float Speed = 1;
        
        public class Baker : Baker<RandomVelocityDirectionOnSpawnAuthoring>
        {
            public override void Bake(RandomVelocityDirectionOnSpawnAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new RandomVelocityDirectionOnSpawnComponent(authoring.Speed));
            }
        }
    }
}