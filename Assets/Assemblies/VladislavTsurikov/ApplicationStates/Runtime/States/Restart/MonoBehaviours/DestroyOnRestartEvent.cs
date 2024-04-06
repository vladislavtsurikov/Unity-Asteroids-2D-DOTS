using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Systems;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.Restart.MonoBehaviours
{
    public class DestroyOnRestartEvent : MonoBehaviour
    {
        private World _world;
        private RestartSystem _system;

        private void OnEnable()
        {
            _world = World.DefaultGameObjectInjectionWorld;
            _system = _world.GetExistingSystemManaged<RestartSystem>();

            _system.RestartEvent += DestroyGameObject;
        }
        
        private void OnDisable()
        {
            _system.RestartEvent -= DestroyGameObject;
        }

        private void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}