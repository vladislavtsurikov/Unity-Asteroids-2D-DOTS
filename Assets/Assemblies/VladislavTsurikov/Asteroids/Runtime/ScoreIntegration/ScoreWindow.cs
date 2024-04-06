using TMPro;
using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Score.Runtime;

namespace VladislavTsurikov.Asteroids.Runtime.ScoreIntegration
{
    public class ScoreWindow : MonoBehaviour
    {
        private EntityManager _entityManager;
        private Entity _scoreEntity;
        
        private void OnEnable()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            _entityManager = world.EntityManager;

            TextMeshProUGUI textMeshProUGUI = gameObject.GetComponentInChildren<TextMeshProUGUI>();

            _scoreEntity = _entityManager.CreateEntityQuery(typeof(ScoreComponent)).GetSingletonEntity();
            ScoreComponent scoreComponent = _entityManager.GetComponentData<ScoreComponent>(_scoreEntity);
            
            textMeshProUGUI.text = scoreComponent.Score.ToString();
        }

        private void OnDisable()
        {
            ScoreComponent scoreComponent = _entityManager.GetComponentData<ScoreComponent>(_scoreEntity);
            scoreComponent.Score = 0;
            _entityManager.SetComponentData(_scoreEntity, scoreComponent);
        }
    }
}