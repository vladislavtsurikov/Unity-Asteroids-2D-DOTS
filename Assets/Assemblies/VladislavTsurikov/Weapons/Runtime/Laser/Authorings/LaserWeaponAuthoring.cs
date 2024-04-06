using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Weapons.Laser.Components;

namespace VladislavTsurikov.Weapons.Laser.Authorings
{
    public class LaserWeaponAuthoring : MonoBehaviour
    {
        public float CooldownShots = 0.1f; //Time between shots

        public int MaxCharges = 3;
        public float RechargeTime = 2;
        public int CurrentCharges = 3;
        
        public int Damage = 100;
        public float Speed = 15;
        public GameObject Prefab;

        public class Baker : Baker<LaserWeaponAuthoring>
        {
            public override void Bake(LaserWeaponAuthoring authoring)
            {
                GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new LaserWeaponComponent()
                {
                    CooldownShots = authoring.CooldownShots,
                    Speed = authoring.Speed,
                    Damage = authoring.Damage,
                    MaxCharges = authoring.MaxCharges,
                    CurrentCharges = Mathf.Min(authoring.CurrentCharges, authoring.MaxCharges),
                    RechargeTime = authoring.RechargeTime,
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}