using Unity.Entities;
using Unity.Mathematics;

namespace VladislavTsurikov.Core.Runtime.Components
{
    public struct PrototypeComponent : IBufferElementData
    {
        public Entity Prefab;
        public float3 PrefabExtents;

        public PrototypeComponent(Entity prefab, float3 prefabExtents)
        {
            Prefab = prefab;
            PrefabExtents = prefabExtents;
        }

        public static PrototypeComponent GetRandomPrototype(DynamicBuffer<PrototypeComponent> prototypes, ref Random random)
        {
            var randomIndex = random.NextInt(0, prototypes.Length);
            return prototypes[randomIndex];
        }
    }
}