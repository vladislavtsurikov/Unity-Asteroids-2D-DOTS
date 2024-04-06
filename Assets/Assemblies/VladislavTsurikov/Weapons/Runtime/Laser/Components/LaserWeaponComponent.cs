using Unity.Entities;

namespace VladislavTsurikov.Weapons.Laser.Components
{
    public struct LaserWeaponComponent : IComponentData
    {
        public float CooldownShots; //Time between shots
        public float CooldownShotsTimer;

        public int MaxCharges;
        public int CurrentCharges;
        public float RechargeTime; //This is the time it takes for the laser charge to be restored
        public float RechargeTimer;
        
        public int Damage;
        public float Speed;
        public Entity Prefab;
    }
}