using Unity.Entities;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Authorings
{
    public class SpawnerGameObjectAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        
        public bool Spawn;

#if UNITY_EDITOR
        [MenuItem("GameObject/VladislavTsurikov/SpawnGameObjectAuthoring", false, 14)]
        public static void AddGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("SpawnGameObjectAuthoring");
            go.AddComponent<SpawnerEntityAuthoring>();
        }
#endif

        public class Baker : Baker<SpawnerGameObjectAuthoring>
        {
            public override void Bake(SpawnerGameObjectAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new SpawnerAtPositionComponent()
                {
                    Spawn = authoring.Spawn,
                });
                
                AddComponentObject(entity, new SpawnPrefab()
                {
                    Prefab = authoring.Prefab,
                });
            }
        }
    }
}