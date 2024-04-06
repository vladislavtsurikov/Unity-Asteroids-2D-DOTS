using System;
using UnityEngine;
using MeshUtility = VladislavTsurikov.Utility.Runtime.MeshUtility;

namespace VladislavTsurikov.Core.Runtime
{
    [Serializable]
    public class Prototype
    {
        [HideInInspector]
        [SerializeField]
        private GameObject _prefab;
        
        [HideInInspector]
        public Vector3 PrefabExtents;
        
        public GameObject Prefab
        {
            get => _prefab;
            set
            {
                if (value == null)
                {
                    return;
                }
                
                if (_prefab != value)
                {
                    _prefab = value;
                    Setup();
                    PrefabChanged?.Invoke(_prefab);
                }
            }
        }

        public event Action<GameObject> PrefabChanged;
        
        public void Setup()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                PrefabExtents = MeshUtility.CalculateBoundsInstantiate(Prefab).extents;
            }
#endif
        }
    }
}