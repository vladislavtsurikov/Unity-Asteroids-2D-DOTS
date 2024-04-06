using Unity.Mathematics;
using Unity.Transforms;
using VladislavTsurikov.Utility.Runtime;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats.UIStats
{
    public class PlayerRotationAngleUI : StatsUI
    {
        protected override string StringFormat => "Player Rotation Angle: {0:0}";

        public void SetText(LocalTransform playerLocalTransform)
        {
            quaternion rotation = playerLocalTransform.Rotation;
                        
            float3 eulerAngles = rotation.GetEulerAngles();
            
            SetText(eulerAngles.z);
        }
    }
}