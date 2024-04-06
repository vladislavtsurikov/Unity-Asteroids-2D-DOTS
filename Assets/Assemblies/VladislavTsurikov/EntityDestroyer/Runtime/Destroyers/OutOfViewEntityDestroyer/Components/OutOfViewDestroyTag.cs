using Unity.Entities;
using VladislavTsurikov.EntityOBB.Runtime.Components;

namespace VladislavTsurikov.EntityDestroyer.Runtime.Destroyers.OutOfViewEntityDestroyer.Components
{
    [WriteGroup(typeof(OBBComponent))]
    public struct OutOfViewDestroyTag : IComponentData
    {
    }
}