using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Authorings;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Authorings
{
    [RequireComponent(typeof(SpawnerEntityAuthoring))]
    public class SpawnAtPositionAuthoring : MonoBehaviour
    {
        public float3 SpawnPosition;
        
#if UNITY_EDITOR
        [MenuItem("GameObject/VladislavTsurikov/SpawnAtPosition", false, 14)]
        public static void AddGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("SpawnAtPosition");
            go.AddComponent<IntervalSpawnerAuthoring>();
        }
#endif

        public class Baker : Baker<SpawnAtPositionAuthoring>
        {
            public override void Bake(SpawnAtPositionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new SpawnAtPositionComponent()
                {
                    SpawnPosition = authoring.SpawnPosition,
                });
            }
        }
    }
}