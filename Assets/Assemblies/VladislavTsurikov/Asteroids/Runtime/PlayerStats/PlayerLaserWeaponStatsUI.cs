using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Asteroids.Runtime.PlayerStats.UIStats;
using VladislavTsurikov.Weapons.Laser.Components;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats
{
    public class PlayerLaserWeaponStatsUI : MonoBehaviour
    {
        private EntityManager _entityManager;
        
        private PlayerWeaponLaserChargesUI _playerWeaponLaserChargesUI;
        private PlayerWeaponLaserCooldownUI _playerWeaponLaserCooldownUI;

        private void OnEnable()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            _entityManager = world.EntityManager;
            
            _playerWeaponLaserChargesUI = GetComponentInChildren<PlayerWeaponLaserChargesUI>();
            _playerWeaponLaserCooldownUI = GetComponentInChildren<PlayerWeaponLaserCooldownUI>();
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            NativeArray<Entity> entities = _entityManager.CreateEntityQuery(typeof(LaserWeaponComponent)).ToEntityArray(Allocator.TempJob);

            for (int i = 0; i < entities.Length; i++)
            {
                LaserWeaponComponent laserWeaponComponent = _entityManager.GetComponentData<LaserWeaponComponent>(entities[i]);
                
                _playerWeaponLaserChargesUI.SetText(laserWeaponComponent);
                _playerWeaponLaserCooldownUI.SetText(laserWeaponComponent);
            }

            entities.Dispose();
        }
    }
}