using Unity.Entities;

namespace VladislavTsurikov.EntityEffects.Effects.Fragmentation.Components
{
    public struct FragmentationEntity : IComponentData
    {
        public int SplitCount;
        public float SplitSpeed;
        public float SplitSize;
        public float Health;

        public FragmentationEntity(int splitCount, float splitSpeed, float splitSize, float health)
        {
            SplitCount = splitCount;
            SplitSpeed = splitSpeed;
            SplitSize = splitSize;
            Health = health;
        }
    }
}