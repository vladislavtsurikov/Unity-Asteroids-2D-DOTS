using Unity.Entities;

namespace VladislavTsurikov.MoveTowardsTarget.Runtime.Components
{
    public struct TargetComponent : IComponentData
    {
        public float Speed;
        public Entity TargetEntity;

        public TargetComponent(Entity targetEntity, float speed)
        {
            TargetEntity = targetEntity;
            Speed = speed;
        }
    }
}