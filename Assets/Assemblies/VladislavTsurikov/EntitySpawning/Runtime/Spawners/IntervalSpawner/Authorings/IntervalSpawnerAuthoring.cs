using System.Collections.Generic;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.Core.Runtime;
using VladislavTsurikov.Core.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Components;
using VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Components;
using Random = Unity.Mathematics.Random;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.IntervalSpawner.Authorings
{
    [ExecuteInEditMode]
    public class IntervalSpawnerAuthoring : MonoBehaviour
    {
        public List<Prototype> Prototypes;

        public uint RandomSeed;
        public float SpawnInterval = 1;
        public bool SpawnAtStart;

#if UNITY_EDITOR
        [MenuItem("GameObject/VladislavTsurikov/IntervalSpawner", false, 14)]
        public static void AddGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("IntervalSpawner");
            go.AddComponent<IntervalSpawnerAuthoring>();
        }
#endif

        private void OnEnable()
        {
            foreach (var proto in Prototypes)
            {
                proto.Setup();
            }
        }

        public class Baker : Baker<IntervalSpawnerAuthoring>
        {
            public override void Bake(IntervalSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new IntervalSpawnerComponent()
                {
                    SpawnInterval = authoring.SpawnInterval,
                    SpawnAtStart = authoring.SpawnAtStart
                });
                
                AddComponent(entity, new SpawnerHashCode() { Value = authoring.GetHashCode() });
                AddComponent(entity, new SpawnerRandom { Value = Random.CreateFromIndex(authoring.RandomSeed) });
                
                DynamicBuffer<PrototypeComponent> prototypeComponents = AddBuffer<PrototypeComponent>(entity);

                foreach (var prototype in authoring.Prototypes)
                {
                    prototypeComponents.Add(new PrototypeComponent(
                        GetEntity(prototype.Prefab, TransformUsageFlags.Dynamic), 
                        prototype.PrefabExtents));
                }
            }
        }
    }
}