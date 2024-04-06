using Unity.Transforms;
using UnityEngine;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.EntityCameraProperties.Runtime.Utility;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats.UIStats
{
    public class PlayerCoordinatesUI : StatsUI
    {
        protected override string StringFormat => "Player Coordinates: ({0:F1}, {1:F1})";

        public void SetText(LocalTransform playerLocalTransform, CameraPropertiesComponent cameraProperties)
        {
            Vector3 screenPoint = CameraUtils.WorldToViewportPoint(playerLocalTransform.Position, cameraProperties);

            SetText(screenPoint.x, screenPoint.y);
        }
    }
}