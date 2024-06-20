using VladislavTsurikov.Weapons.Runtime.Laser.Components;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats.UIStats
{
    public class PlayerWeaponLaserCooldownUI : StatsUI
    {
        protected override string StringFormat => "Laser Cooldown: {0:F1}";
        
        public void SetText(LaserWeaponComponent laserWeaponComponent)
        {
            SetText(laserWeaponComponent.RechargeTime);
        }
    }
}