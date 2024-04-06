using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using VladislavTsurikov.Asteroids.Runtime.PlayerStats.UIStats;
using VladislavTsurikov.EntityCameraProperties.Runtime.Components;
using VladislavTsurikov.PhysicsVelocity.Runtime.Components;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats
{
    public class PlayerControllerStatsUI : MonoBehaviour
    {
        private EntityManager _entityManager;
        
        private PlayerCoordinatesUI _playerCoordinatesUI;
        private PlayerRotationAngleUI _playerRotationAngleUI;
        private PlayerVelocityUI _playerVelocityUI;

        private void OnEnable()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            _entityManager = world.EntityManager;
            
            _playerCoordinatesUI = GetComponentInChildren<PlayerCoordinatesUI>();
            _playerVelocityUI = GetComponentInChildren<PlayerVelocityUI>();
            _playerRotationAngleUI = GetComponentInChildren<PlayerRotationAngleUI>();
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            Entity cameraPropertiesEntity = _entityManager.CreateEntityQuery(typeof(CameraPropertiesComponent)).GetSingletonEntity();
            CameraPropertiesComponent cameraProperties = _entityManager.GetComponentData<CameraPropertiesComponent>(cameraPropertiesEntity);

            NativeArray<Entity> playerEntities = _entityManager.CreateEntityQuery(typeof(global::VladislavTsurikov._2DSpaceshipController.Runtime.Components.SpaceshipController), typeof(LocalTransform), typeof(CustomPhysicsVelocity)).ToEntityArray(Allocator.TempJob);

            for (int i = 0; i < playerEntities.Length; i++)
            {
                Entity playerEntity = playerEntities[i];
                LocalTransform localTransform = _entityManager.GetComponentData<LocalTransform>(playerEntity);
                CustomPhysicsVelocity customPhysicsVelocity = _entityManager.GetComponentData<CustomPhysicsVelocity>(playerEntity);
                
                _playerCoordinatesUI.SetText(localTransform, cameraProperties);
                _playerRotationAngleUI.SetText(localTransform);
                _playerVelocityUI.SetText(customPhysicsVelocity);
            }

            playerEntities.Dispose();
        }
    }
}