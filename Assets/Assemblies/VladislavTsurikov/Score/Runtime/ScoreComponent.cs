using Unity.Entities;

namespace VladislavTsurikov.Score.Runtime
{
    public struct ScoreComponent : IComponentData
    {
        public int Score;
    }
}