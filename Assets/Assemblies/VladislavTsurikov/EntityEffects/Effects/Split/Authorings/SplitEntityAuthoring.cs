using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.Core.Runtime;
using VladislavTsurikov.Core.Runtime.Components;
using VladislavTsurikov.EntityEffects.Effects.Split.Components;

namespace VladislavTsurikov.EntityEffects.Effects.Split.Authorings
{
    public class SplitEntityAuthoring : MonoBehaviour
    {
        public List<Prototype> Prototypes = new List<Prototype>();
        public int SplitCount = 3;
        public float SplitSpeed;
        public float SplitSize;
        public float Health;

        public class Baker : Baker<SplitEntityAuthoring>
        {
            public override void Bake(SplitEntityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity,
                    new SplitEntity
                        {
                            SplitCount = authoring.SplitCount,
                            SplitSpeed = authoring.SplitSpeed,
                            SplitSize = authoring.SplitSize,
                            Health = authoring.Health,
                        });
                
                DynamicBuffer<PrototypeComponent> prototypeComponents = AddBuffer<PrototypeComponent>(entity);
                
                 foreach (var prototype in authoring.Prototypes)
                 {
                     prototypeComponents.Add(new PrototypeComponent
                     {
                         Prefab = GetEntity(prototype.Prefab, TransformUsageFlags.Dynamic), 
                     });
                 }
            }
        }
    }
}