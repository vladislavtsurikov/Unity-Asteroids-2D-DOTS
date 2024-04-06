using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Authorings
{
    public class SpawnOnRestartEventAuthoring : MonoBehaviour
    {
        public class Baker : Baker<SpawnOnRestartEventAuthoring>
        {
            public override void Bake(SpawnOnRestartEventAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new SpawnOnRestartEventComponent());
            }
        }
    }
}