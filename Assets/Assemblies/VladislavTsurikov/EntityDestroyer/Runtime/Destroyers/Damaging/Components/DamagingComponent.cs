using Unity.Entities;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.Damaging.Components
{
    public struct DamagingComponent : IComponentData
    {
        public float DamageValue;
        public bool DestroyDamagingEntity;
    }
}