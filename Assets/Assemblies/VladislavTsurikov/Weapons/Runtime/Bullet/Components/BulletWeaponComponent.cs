using Unity.Entities;

namespace VladislavTsurikov.Weapons.Runtime.Bullet.Components
{
    public struct BulletWeaponComponent : IComponentData
    {
        public float CooldownShots;
        public float CooldownShotsTimer; //Timer to track time until next shot
        public float Speed;
        public float Damage;
        public Entity Prefab;
    }
}