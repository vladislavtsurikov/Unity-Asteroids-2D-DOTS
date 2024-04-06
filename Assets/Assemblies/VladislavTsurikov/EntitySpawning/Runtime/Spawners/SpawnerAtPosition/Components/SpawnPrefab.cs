using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace VladislavTsurikov.EntitySpawning.Runtime.Spawners.SpawnerAtPosition.Components
{
    public class SpawnPrefab : IComponentData
    {
        private GameObject _instancePrefab;
        
        public GameObject Prefab;

        public GameObject SpawnAtPosition(float3 position)
        {
            GameObject go = Spawn();
            
            go.transform.position = position;

            return go;
        }

        public GameObject Spawn()
        {
            _instancePrefab = Object.Instantiate(Prefab);
            return _instancePrefab;
        }

        public void DestroyInstance()
        {
            if (_instancePrefab == null)
            {
                return;
            }
            
            Object.Destroy(_instancePrefab);
        }
    }
}