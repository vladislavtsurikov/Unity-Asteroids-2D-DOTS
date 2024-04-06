using Unity.Entities;
using UnityEngine;
using VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Components;

namespace VladislavTsurikov.ApplicationStates.Runtime.States.Restart.Authorings
{
    public class DestroyEntityOnRestartAuthoring : MonoBehaviour
    {
        public class Baker : Baker<DestroyEntityOnRestartAuthoring>
        {
            public override void Bake(DestroyEntityOnRestartAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new DestroyOnRestart());
            }
        }
    }
}