using Unity.Entities;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.GameOver.Authorings
{
    public class GameOverAuthoring : MonoBehaviour
    {
        
#if UNITY_EDITOR
        [MenuItem("GameObject/VladislavTsurikov/ApplicationStates/GameOverAuthoring", false, 14)]
        public static void AddGameObject(MenuCommand menuCommand)
        {
            GameObject obj = new GameObject("GameOverAuthoring");
            obj.AddComponent<GameOverAuthoring>();
        }
#endif
        public class Baker : Baker<GameOverAuthoring>
        {
            public override void Bake(GameOverAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new GameOverEnableableComponent());
                SetComponentEnabled<GameOverEnableableComponent>(entity, false);
            }
        }
    }
}