using Unity.Entities;

namespace VladislavTsurikov.EntityEffects.Effects.Split.Components
{
    public struct SplitEntity : IComponentData
    {
        public int SplitCount;
        public float SplitSpeed;
        public float SplitSize;
        public float Health;

        public SplitEntity(int splitCount, float splitSpeed, float splitSize, float health)
        {
            SplitCount = splitCount;
            SplitSpeed = splitSpeed;
            SplitSize = splitSize;
            Health = health;
        }
    }
}