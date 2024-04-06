using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace VladislavTsurikov.EntityCameraProperties.Runtime.Authorings
{
    public class CameraAuthoring : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/VladislavTsurikov/CameraAuthoring", false, 14)]
        public static void AddGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("CameraAuthoring");
            go.AddComponent<CameraAuthoring>();
        }
#endif
        
        public class Baker : Baker<CameraAuthoring>
        {
            public override void Bake(CameraAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new Components.CameraPropertiesComponent());
            }
        }
    }
}