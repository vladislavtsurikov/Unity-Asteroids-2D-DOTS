using Unity.Entities;

namespace VladislavTsurikov.Asteroids.Runtime.Components
{
    public struct RandomVelocityDirectionOnSpawnComponent : IComponentData
    {
        public float Speed;

        public RandomVelocityDirectionOnSpawnComponent(float speed)
        {
            Speed = speed;
        }
    }
}