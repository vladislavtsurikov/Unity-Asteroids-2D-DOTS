using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Systems;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.GameOver
{
    public class DestroyOnGameOverEvent : MonoBehaviour
    {
        private World _world;
        private EnableGameOverSystem _enableGameOverSystem;

        private void OnEnable()
        {
            _world = World.DefaultGameObjectInjectionWorld;
            _enableGameOverSystem = _world.GetExistingSystemManaged<EnableGameOverSystem>();

            _enableGameOverSystem.GameOverEvent += DestroyEnableGameObject;
        }
        
        private void OnDisable()
        {
            _enableGameOverSystem.GameOverEvent -= DestroyEnableGameObject;
        }

        private void DestroyEnableGameObject()
        {
            Destroy(gameObject);
        }
    }
}