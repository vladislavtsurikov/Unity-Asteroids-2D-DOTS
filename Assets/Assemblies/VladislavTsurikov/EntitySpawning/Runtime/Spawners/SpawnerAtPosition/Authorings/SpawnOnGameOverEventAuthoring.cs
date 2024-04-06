using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Authorings
{
    public class SpawnOnGameOverEventAuthoring : MonoBehaviour
    {
        public class Baker : Baker<SpawnOnGameOverEventAuthoring>
        {
            public override void Bake(SpawnOnGameOverEventAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new SpawnOnGameOverEventComponent());
            }
        }
    }
}