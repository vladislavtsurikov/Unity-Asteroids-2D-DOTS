using VladislavTsurikov.Weapons.Runtime.Laser.Components;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats.UIStats
{
    public class PlayerWeaponLaserChargesUI : StatsUI
    {
        protected override string StringFormat => "Laser Charges: {0:0}";

        public void SetText(LaserWeaponComponent laserWeaponComponent)
        {
            SetText(laserWeaponComponent.CurrentCharges);
        }
    }
}