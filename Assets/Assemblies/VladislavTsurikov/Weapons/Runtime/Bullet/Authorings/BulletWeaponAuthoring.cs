using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Utility.Runtime;
using VladislavTsurikov.Weapons.Runtime.Bullet.Components;

namespace VladislavTsurikov.Weapons.Runtime.Bullet.Authorings
{
    [ExecuteInEditMode]
    public class BulletWeaponAuthoring : MonoBehaviour
    {
        public float CooldownShots = 0.1f;
        public float Speed = 10;
        public float Damage = 1;
        public GameObject Prefab;
        
        [SerializeField] 
        internal Vector3 _prefabExtents;
        
        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                _prefabExtents = MeshUtility.CalculateBoundsInstantiate(Prefab).extents;
            }
#endif
        }
        
        public class Baker : Baker<BulletWeaponAuthoring>
        {
            public override void Bake(BulletWeaponAuthoring authoring)
            {
                GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new BulletWeaponComponent
                {
                    CooldownShots = authoring.CooldownShots,
                    Speed = authoring.Speed,
                    Damage = authoring.Damage,
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}