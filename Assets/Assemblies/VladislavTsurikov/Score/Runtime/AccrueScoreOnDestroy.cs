using Unity.Entities;

namespace VladislavTsurikov.Score.Runtime
{
    public struct AccrueScoreOnDestroy : IComponentData
    {
        public int Score;

        public AccrueScoreOnDestroy(int score)
        {
            Score = score;
        }
    }
}