using Unity.Entities;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Components
{
    public struct IntervalSpawnerComponent : IComponentData
    {
        private float _timer;
        
        public float SpawnInterval;
        public bool SpawnAtStart;
        
        public bool IsSpawned { get; internal set; }

        public void UpdateTimer(float deltaTime)
        {
            _timer += deltaTime;
        }

        public bool CanSpawn()
        {
            if (!IsSpawned)
            {
                if (SpawnAtStart)
                {
                    return true;
                }
            }

            return _timer >= SpawnInterval;
        }

        public void ResetTimer()
        {
            _timer = 0f;
        }

        internal void Reset()
        {
            ResetTimer();
            IsSpawned = false;
        }
    }
}