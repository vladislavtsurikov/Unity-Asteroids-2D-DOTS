using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Score.Runtime;

namespace VladislavTsurikov.Score.Authorings
{
    public class ScoreAuthoring : MonoBehaviour
    {
        public class Baker : Baker<ScoreAuthoring>
        {
            public override void Bake(ScoreAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new ScoreComponent());
            }
        }
    }
}