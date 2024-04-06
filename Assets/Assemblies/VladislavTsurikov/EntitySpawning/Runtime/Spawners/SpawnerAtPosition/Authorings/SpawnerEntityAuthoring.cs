using Unity.Entities;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.Core.Runtime;
using VladislavTsurikov.Core.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Authorings
{
    public class SpawnerEntityAuthoring : MonoBehaviour
    {
        public Prototype Prototype = new Prototype();
        
        public bool SpawnAtOnce;

#if UNITY_EDITOR
        [MenuItem("GameObject/VladislavTsurikov/SpawnAuthoring", false, 14)]
        public static void AddGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("SpawnAuthoring");
            go.AddComponent<SpawnerEntityAuthoring>();
        }
#endif

        public class Baker : Baker<SpawnerEntityAuthoring>
        {
            public override void Bake(SpawnerEntityAuthoring entityAuthoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new SpawnerAtPositionComponent()
                {
                    Spawn = entityAuthoring.SpawnAtOnce,
                });

                DynamicBuffer<PrototypeComponent> prototypeComponents = AddBuffer<PrototypeComponent>(entity);

                prototypeComponents.Add(new PrototypeComponent
                {
                    Prefab = GetEntity(entityAuthoring.Prototype.Prefab, TransformUsageFlags.Dynamic), 
                });
            }
        }
    }
}