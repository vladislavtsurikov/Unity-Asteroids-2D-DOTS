using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;

namespace VladislavTsurikov.Asteroids.Runtime.ApplicationStatesIntegration.GameOver.MonoBehaviours
{
    [RequireComponent(typeof(Canvas))]
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] 
        private Button _restart;

        private EntityManager _entityManager;
        
        private void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _restart.onClick.AddListener(OnRestartClicked);
        }
        
        private void OnDisable()
        {
            _restart.onClick.RemoveListener(OnRestartClicked);
        }

        private void OnRestartClicked()
        {
            _entityManager.CreateEntity(typeof(RestartEvent));

            Destroy(gameObject);
        }
    }
}