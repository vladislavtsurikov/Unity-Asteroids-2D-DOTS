using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Score.Runtime;

namespace VladislavTsurikov.Score.Authorings
{
    public class AccrueScoreOnDestroyAuthoring : MonoBehaviour
    {
        public int Score;
        
        public class Baker : Baker<AccrueScoreOnDestroyAuthoring>
        {
            public override void Bake(AccrueScoreOnDestroyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new AccrueScoreOnDestroy(authoring.Score));
            }
        }
    }
}