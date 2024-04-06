using Unity.Entities;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov._2DSpaceshipController.Runtime.Components;
using VladislavTsurikov.PhysicsVelocity.Runtime.Components;

namespace VladislavTsurikov._2DSpaceshipController.Runtime.Authorings
{
    [ExecuteInEditMode]
    public class SpaceshipControllerAuthoring : MonoBehaviour
    {
        public float Acceleration = 12;
        public float RotationSpeed = 250;
        
#if UNITY_EDITOR
        [MenuItem("GameObject/VladislavTsurikov/PlayerController", false, 14)]
        public static void AddGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("PlayerController");
            go.AddComponent<SpaceshipControllerAuthoring>();
        }
#endif

        public class Baker : Baker<SpaceshipControllerAuthoring>
        {
            public override void Bake(SpaceshipControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new SpaceshipController()
                {
                    MoveAcceleration = authoring.Acceleration,
                    RotationSpeed = authoring.RotationSpeed
                });
                AddComponent(entity, new SpaceshipControllerInput());
                AddComponent(entity, new CustomPhysicsVelocity());
            }
        }
    }
}