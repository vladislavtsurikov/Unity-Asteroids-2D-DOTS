using Unity.Entities;

namespace VladislavTsurikov.Asteroids.Runtime.Components
{
    public struct MoveToPlayerComponent : IComponentData
    {
        public float Speed;

        public MoveToPlayerComponent(float speed)
        {
            Speed = speed;
        }
    }
}