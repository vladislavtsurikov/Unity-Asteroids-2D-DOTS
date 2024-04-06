using Unity.Mathematics;
using VladislavTsurikov.PhysicsVelocity.Runtime.Components;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats.UIStats
{
    public class PlayerVelocityUI : StatsUI
    {
        protected override string StringFormat => "Player Velocity: {0:F1}";

        public void SetText(CustomPhysicsVelocity playerPhysicsVelocity)
        {
            SetText(math.length(playerPhysicsVelocity.Linear));
        }
    }
}